using System.Threading.Tasks;

namespace AliasPro.Item.Packets.Incoming
{
    using Models;
    using Room.Models;
    using Room.Gamemap;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Outgoing;
    using Sessions;

    public class PlaceItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.PlaceItemMessageEvent;
        
        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string rawData = clientPacket.ReadString();
            string[] data = rawData.Split(' ');

            uint itemId = uint.Parse(data[0]);

            IRoom room = session.CurrentRoom;
            
            if (!room.RightHandler.HasRights(session.Player.Id)) return;

            if (session.Player.Inventory.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.Type == "s")
                {
                    int x = int.Parse(data[1]);
                    int y = int.Parse(data[2]);
                    int rot = int.Parse(data[3]);

                    if (!room.RoomMap.TryGetRoomTile(x, y, out RoomTile roomTile)) return;

                    if (!room.RoomMap.CanStackAt(x, y, item)) return;
                    
                    item.Position.X = x;
                    item.Position.Y = y;
                    item.Position.Z = roomTile.Height;
                    item.Rotation = rot;
                    room.RoomMap.AddItem(item);

                    await room.SendAsync(new ObjectAddComposer(item));
                }
                else
                {
                    item.ExtraData = data[1] + " " + data[2] + " " + data[3];
                    await session.SendPacketAsync(new AddWallItemComposer(item));
                }
                
                item.RoomId = room.RoomData.Id;
                room.ItemHandler.AddItem(item);
                
                await session.Player.Inventory.RemoveItem(item);
            }
        }
    }
}
