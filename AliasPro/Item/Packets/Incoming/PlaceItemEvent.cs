using System.Threading.Tasks;

namespace AliasPro.Item.Packets.Incoming
{
    using Models;
    using Room.Models;
    using Room.Gamemap;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
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
            if (session.Player.Inventory.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.Type == "s")
                {
                    int x = int.Parse(data[1]);
                    int y = int.Parse(data[2]);
                    double z = 0.00;
                    int rot = int.Parse(data[3]);

                    if (room.RoomMap.TryGetRoomTile(x, y, out RoomTile roomTile))
                    {
                        IItem topItem = roomTile.GetTopItem();
                        if (topItem != null && topItem != item)
                            z = topItem.Position.Z + topItem.ItemData.Height;
                    }

                    item.RoomId = room.RoomData.Id;
                    item.Position.X = x;
                    item.Position.Y = y;
                    item.Position.Z = z;
                    item.Rotation = rot;

                    await room.AddItem(item);
                }
                else
                {
                    //todo: wall items
                }

                await session.Player.Inventory.RemoveItem(item);
            }
        }
    }
}
