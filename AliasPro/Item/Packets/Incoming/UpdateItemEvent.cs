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

            if (!room.RightHandler.HasRights(session.Player.Id)) return;

            uint itemId = (uint)clientPacket.ReadInt();
            if (room.ItemHandler.TryGetItem(itemId, out IItem item))
            {
                int x = clientPacket.ReadInt();
                int y = clientPacket.ReadInt();
                int rot = clientPacket.ReadInt();
                
                if (room.RoomMap.TryGetRoomTile(x, y, out RoomTile roomTile))
                {
                    if (room.RoomMap.CanStackAt(x, y, item))
                    {
                        room.RoomMap.RemoveItem(item);
                        item.RoomId = room.RoomData.Id;
                        item.Position = new Position(
                            x,
                            y,
                            roomTile.Height);
                        item.Rotation = rot;
                        room.RoomMap.AddItem(item);
                    }
                }

                await room.SendAsync(new FloorItemUpdateComposer(item));
            }
        }
    }
}
