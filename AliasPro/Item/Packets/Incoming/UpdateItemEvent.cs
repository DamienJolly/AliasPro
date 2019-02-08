using System.Threading.Tasks;

namespace AliasPro.Item.Packets.Incoming
{
    using Models;
    using Room.Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Outgoing;
    using Room.Gamemap;

    public class UpdateItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UpdateItemMessageEvent;
        
        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            uint itemId = (uint)clientPacket.ReadInt();
            if (room.ItemHandler.TryGetItem(itemId, out IItem item))
            {
                int x = clientPacket.ReadInt();
                int y = clientPacket.ReadInt();
                double z = 0.00;
                int rot = clientPacket.ReadInt();

                if (!room.RoomMap.TryGetRoomTile(x, y, out RoomTile roomTile)) return;
                
                IItem topItem = roomTile.GetTopItem();
                if (topItem != null && topItem != item)
                    z = topItem.Position.Z + topItem.ItemData.Height;

                room.RemoveItem(item);
                item.Position = new Position(x, y, z);
                item.RoomId = room.RoomData.Id;
                item.Rotation = rot;
                room.AddItem(item);

                await room.SendAsync(new FloorItemUpdateComposer(item));
            }
        }
    }
}
