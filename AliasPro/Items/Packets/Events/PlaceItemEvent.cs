using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Models;
using AliasPro.Sessions;

namespace AliasPro.Items.Packets.Events
{
    public class PlaceItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.PlaceItemMessageEvent;
        
        public async void HandleAsync(
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
                item.CurrentRoom = room;

                room.ItemHandler.AddItem(item);
                session.Player.Inventory.RemoveItem(item.Id);

                await session.SendPacketAsync(new RemovePlayerItemComposer(item.Id));
            }
        }
    }
}
