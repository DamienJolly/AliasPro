using AliasPro.API.Crafting;
using AliasPro.API.Crafting.Models;
using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Crafting.Packets.Composers;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Crafting.Packets.Events
{
	public class CraftingCraftItemEvent : IMessageEvent
	{
		public short Header => Incoming.CraftingCraftItemMessageEvent;

		private readonly ICraftingController _craftingController;
		private readonly IItemController _itemController;

		public CraftingCraftItemEvent(
			ICraftingController craftingController,
			IItemController itemController)
		{
			_craftingController = craftingController;
            _itemController = itemController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
		{
			IRoom room = session.CurrentRoom;
			if (room == null)
				return;

            int itemId = message.ReadInt();
            if (!room.Items.TryGetItem((uint)itemId, out IItem item))
                return;

            if (!_craftingController.TryGetAltar((int)item.ItemData.Id, out ICraftingAltar altar))
                return;

            string recipeName = message.ReadString();
            if (!altar.TryGetRecipe(recipeName, out ICraftingRecipe recipe))
            {
                ReturnItems(session, recipe, null);
                return;
            }

            if (!recipe.CanBeCrafted)
            {
                await session.SendPacketAsync(new AlertLimitedSoldOutComposer());
                ReturnItems(session, recipe, null);
                return;
            }

            int ingredientsCount = 0;
            IList<IItem> toRemove = new List<IItem>();
            foreach (ICraftingIngredient ingredient in recipe.Ingredients.Values)
            {
                for (int i = 0; i < ingredient.Amount; i++)
                {
                    ingredientsCount++;

                    if (!session.Player.Inventory.TryGetItemByBase((uint)ingredient.Id, out IItem playerItem))
                        continue;

                    toRemove.Add(playerItem);
                    session.Player.Inventory.RemoveItem(playerItem.Id);
                }
            }

            if (toRemove.Count != ingredientsCount)
            {
                ReturnItems(session, recipe, toRemove);
                return;
            }

            if (!_itemController.TryGetItemDataById((uint)recipe.RewardId, out IItemData rewardItemData))
            {
                ReturnItems(session, recipe, toRemove);
                return;
            }

            IItem rewardItem = new Item(rewardItemData.Id, session.Player.Id, session.Player.Username, "", rewardItemData);
            rewardItem.Id = (uint)await _itemController.AddNewItemAsync(rewardItem);
            session.Player.Inventory.TryAddItem(rewardItem);

            await session.SendPacketAsync(new AddPlayerItemsComposer(1, (int)rewardItem.Id));
            await session.SendPacketAsync(new CraftingResultComposer(recipe, true));
            await session.SendPacketAsync(new InventoryRefreshComposer());

            foreach (IItem i in toRemove)
            {
                await _itemController.RemoveItemAsync(i);
                await session.SendPacketAsync(new RemovePlayerItemComposer(i.Id));
            }

            if (recipe.Limited)
            {
                recipe.Remaining--;
                await _craftingController.UpdateRecipeAsync(recipe);
            }

            if (recipe.Achievement != "")
            {
                //todo: some achievement
            }
		}

        public async void ReturnItems(ISession session, ICraftingRecipe recipe, IList<IItem> items)
        {
            foreach (IItem i in items)
                session.Player.Inventory.TryAddItem(i);

            await session.SendPacketAsync(new CraftingResultComposer(recipe, false));
        }
    }
}
