using AliasPro.API.Crafting;
using AliasPro.API.Crafting.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Crafting.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Crafting.Packets.Events
{
	public class RequestCraftingRecipesAvailableEvent : IMessageEvent
	{
		public short Header => Incoming.RequestCraftingRecipesAvailableMessageEvent;

		private readonly ICraftingController _craftingController;

		public RequestCraftingRecipesAvailableEvent(
			ICraftingController craftingController)
		{
			_craftingController = craftingController;
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

            IDictionary<int, int> playerItems = new Dictionary<int, int>();
            int count = message.ReadInt();
            for (int i = 0; i < count; i++)
            {
                if (!session.Player.Inventory.TryGetItem((uint)message.ReadInt(), out IItem playerItem))
                    continue;

                if (!playerItems.ContainsKey((int)playerItem.ItemData.Id))
                    playerItems.Add((int)playerItem.ItemData.Id, 0);

                playerItems[(int)playerItem.ItemData.Id]++;
            }

            IDictionary<ICraftingRecipe, bool> recipes = altar.MatchRecipes(playerItems);
            bool found = false;
            foreach (var r in recipes)
            {
                if (r.Value)
                {
                    found = true;
                    break;
                }
            }

            await session.SendPacketAsync(new CraftingRecipesAvailableComposer(recipes.Count, found));
		}
	}
}
