using AliasPro.API.Crafting;
using AliasPro.API.Crafting.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Crafting.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Crafting.Packets.Events
{
	public class RequestCraftingRecipesEvent : IMessageEvent
	{
		public short Header => Incoming.RequestCraftingRecipesMessageEvent;

		private readonly ICraftingController _craftingController;

		public RequestCraftingRecipesEvent(
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

			await session.SendPacketAsync(new CraftableProductsComposer(altar.GetRecipesForPlayer(session.Player), altar.Ingredients));
		}
	}
}
