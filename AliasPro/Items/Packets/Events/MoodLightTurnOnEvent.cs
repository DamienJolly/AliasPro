using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class MoodLightTurnOnEvent : IMessageEvent
    {
        public short Header => Incoming.MoodLightTurnOnMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;

            if (room == null || session.Entity == null)
                return;

            if (room.Moodlight == null)
                return;

            if (!room.Rights.HasRights(session.Player.Id))
                return;

            room.Moodlight.Enabled = !room.Moodlight.Enabled;

            foreach (IItem item in room.Items.GetItemsByType(ItemInteractionType.DIMMER))
            {
                item.ExtraData = room.Moodlight.GenerateExtraData;
                await room.SendPacketAsync(new WallItemUpdateComposer(item));
            }
        }
    }
}
