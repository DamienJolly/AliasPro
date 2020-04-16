using AliasPro.API.Crafting;
using AliasPro.API.Crafting.Models;
using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Players;
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
	public class CraftingCraftSecretEvent : IMessageEvent
	{
		public short Header => Incoming.CraftingCraftSecretMessageEvent;

		private readonly ICraftingController _craftingController;
		private readonly IItemController _itemController;
		private readonly IPlayerController _playerController;

		public CraftingCraftSecretEvent(
			ICraftingController craftingController,
			IItemController itemController,
			IPlayerController playerController)
		{
			_craftingController = craftingController;
            _itemController = itemController;
            _playerController = playerController;
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

            int count = message.ReadInt();

            IList<IItem> toRemove = new List<IItem>();
            IDictionary<int, int> playerItems = new Dictionary<int, int>();
            for (int i = 0; i < count; i++)
            {
                int ingredientId = message.ReadInt();
                if (!session.Player.Inventory.TryGetItem((uint)ingredientId, out IItem playerItem))
                    continue;

                toRemove.Add(playerItem);
                session.Player.Inventory.RemoveItem(playerItem.Id);

                if (!playerItems.ContainsKey((int)playerItem.ItemData.Id))
                    playerItems.Add((int)playerItem.ItemData.Id, 0);

                playerItems[(int)playerItem.ItemData.Id]++;
            }

            if (toRemove.Count != count)
            {
                ReturnItems(session, null, toRemove);
                return;
            }

            if (!altar.TryGetRecipe(playerItems, out ICraftingRecipe recipe))
            {
                ReturnItems(session, recipe, toRemove);
                return;
            }

            if (session.Player.Recipe.TryGetRecipe(recipe.Id))
            {
                ReturnItems(session, recipe, toRemove);
                return;
            }

            if (!recipe.CanBeCrafted)
            {
                await session.SendPacketAsync(new AlertLimitedSoldOutComposer());
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
                //todo: update in db
            }

            if (recipe.Achievement != "")
            {
                //todo: some achievement
            }

            session.Player.Recipe.TryAdd(recipe.Id);
            await _playerController.AddPlayerRecipeAsync((int)session.Player.Id, recipe.Id);
        }

        public async void ReturnItems(ISession session, ICraftingRecipe recipe, IList<IItem> items)
        {
            foreach (IItem i in items)
                session.Player.Inventory.TryAddItem(i);

            await session.SendPacketAsync(new CraftingResultComposer(recipe, false));
        }
	}
}
