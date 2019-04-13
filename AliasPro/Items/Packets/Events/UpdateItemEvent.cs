using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Models;

namespace AliasPro.Items.Packets.Events
{
    public class UpdateItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UpdateItemMessageEvent;
        
        public async void HandleAsync(
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
