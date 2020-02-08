using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class RedeemItemEvent : IMessageEvent
    {
        public short Header => Incoming.RedeemItemMessageEvent;

		private readonly IItemController _itemController;

		public RedeemItemEvent(IItemController itemController)
		{
			_itemController = itemController;
		}

		public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) 
                return;

            if (session.Entity == null)
                return;

            if (!room.Rights.IsOwner(session.Player.Id)) 
                return;

            uint itemId = (uint)clientPacket.ReadInt();
            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.Type != "s") 
                    return;

                if (item.ItemData.InteractionType == ItemInteractionType.EXCHANGE) 
                    return;

                item.Interaction.OnUserInteract(session.Entity);
				room.Items.TriggerWired(WiredInteractionType.STATE_CHANGED, session.Entity, item);

				room.RoomGrid.RemoveItem(item);
				room.Items.RemoveItem(item.Id);
				await room.SendPacketAsync(new RemoveFloorItemComposer(item));
				await _itemController.RemoveItemAsync(item);
            }
        }
    }
}
