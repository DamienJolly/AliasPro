using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class UpdateWallEvent : IMessageEvent
    {
        public short Header => Incoming.UpdateWallMessageEvent;
        
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
                    string wallPosition = message.ReadString();
                    item.WallCord = wallPosition;
                    item.Interaction.OnMoveItem();
                }

				await room.SendPacketAsync(new WallItemUpdateComposer(item));
            }
        }
    }
}
