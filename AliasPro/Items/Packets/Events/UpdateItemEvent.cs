using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Models;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class UpdateItemEvent : IMessageEvent
    {
        public short Header => Incoming.UpdateItemMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;

            uint itemId = (uint)message.ReadInt();
            if (room.Items.TryGetItem(itemId, out IItem item))
            {
                if (room.Rights.HasRights(session.Player.Id))
                {
                    int x = message.ReadInt();
                    int y = message.ReadInt();
                    int rot = message.ReadInt();

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

                await room.SendPacketAsync(new FloorItemUpdateComposer(item));
            }
        }
    }
}
