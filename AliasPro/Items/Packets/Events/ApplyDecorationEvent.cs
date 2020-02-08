using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class ApplyDecorationEvent : IMessageEvent
    {
        public short Header => Incoming.ApplyDecorationMessageEvent;

		private readonly IItemController _itemController;

		public ApplyDecorationEvent(IItemController itemController)
		{
			_itemController = itemController;
		}

		public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) 
                return;

            if (session.Entity == null) 
                return;

            if (!room.Rights.IsOwner(session.Player.Id)) 
                return;

            uint itemId = (uint)message.ReadInt();
            if (session.Player.Inventory.TryGetItem(itemId, out IItem item))
            {
                switch (item.ItemData.InteractionType)
                {
                    case ItemInteractionType.WALLPAPER:
                        room.WallPaint = item.ExtraData;
                        await room.SendPacketAsync(new RoomPaintComposer("wallpaper", room.WallPaint));
                        break;

                    case ItemInteractionType.FLOOR:
                        room.FloorPaint = item.ExtraData;
                        await room.SendPacketAsync(new RoomPaintComposer("floor", room.FloorPaint));
                        break;

                    case ItemInteractionType.LANDSCAPE:
                        room.BackgroundPaint = item.ExtraData;
                        await room.SendPacketAsync(new RoomPaintComposer("landscape", room.BackgroundPaint));
                        break;
                    default: return;
                }

                session.Player.Inventory.RemoveItem(item.Id);
                await _itemController.RemoveItemAsync(item);
                await session.SendPacketAsync(new RemovePlayerItemComposer(item.Id));
            }
        }
    }
}
