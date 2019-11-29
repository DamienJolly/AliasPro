using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Tasks;
using AliasPro.Network.Events.Headers;
using AliasPro.Tasks;

namespace AliasPro.Items.Packets.Events
{
	public class RedeemGiftEvent : IAsyncPacket
	{
        public short Header { get; } = Incoming.RedeemGiftMessageEvent;

		private readonly IItemController _itemController;

		public RedeemGiftEvent(
			IItemController itemController)
		{
			_itemController = itemController;
		}

		public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) return;

			if (!session.CurrentRoom.Rights.IsOwner(session.Player.Id)) return;

			uint itemId = (uint)clientPacket.ReadInt();
			if (!room.Items.TryGetItem(itemId, out IItem item)) return;

			if (item.ItemData.Type != "s") return;

			if (item.Interaction is InteractionGift interaction)
			{
				if (item.ItemData.Name.Contains("present_wrap"))
				{
					interaction.Exploaded = true;
					await room.SendAsync(new FloorItemUpdateComposer(item));
				}

				await TaskManager.ExecuteTask(new OpenGiftTask(_itemController, item, session), interaction.Exploaded ? 1500 : 0);
			}
		}
    }
}
