using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Models;

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

            uint itemId = (uint)clientPacket.ReadInt();
            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                if (room.Rights.HasRights(session.Player.Id))
                {
                    int x = clientPacket.ReadInt();
                    int y = clientPacket.ReadInt();
                    int rot = clientPacket.ReadInt();

                    if (room.RoomGrid.TryGetRoomTile(x, y, out IRoomTile roomTile))
                    {
                        if (room.RoomGrid.CanStackAt(x, y, item))
                        {
                            room.RoomGrid.RemoveItem(item);
                            item.RoomId = room.Id;
                            item.Position = new RoomPosition(
                                x,
                                y,
                                item.Position.Z = item.ItemData.InteractionType == Types.ItemInteractionType.STACK_TOOL ? item.Position.Z : roomTile.Height);
                            item.Rotation = rot;
                            room.RoomGrid.AddItem(item);
                            item.Interaction.OnMoveItem();
                        }
                    }
                }

                await room.SendAsync(new FloorItemUpdateComposer(item));
            }
        }
    }
}
