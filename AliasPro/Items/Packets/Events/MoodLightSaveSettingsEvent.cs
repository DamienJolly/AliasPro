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
    public class MoodLightSaveSettingsEvent : IMessageEvent
    {
        public short Header => Incoming.MoodLightSaveSettingsMessageEvent;
        
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

            int id = message.ReadInt();
            bool backgroundOnly = message.ReadInt() >= 2;
            string color = message.ReadString();
            int intensity = message.ReadInt();

            foreach (IRoomMoodlightPreset preset in room.Moodlight.Presets)
            {
                if (id == preset.Id)
                {
                    preset.BackgroundOnly = backgroundOnly;
                    preset.Colour = color;
                    preset.Intensity = intensity;
                    preset.Enabled = true;
                    room.Moodlight.Enabled = true;
                    room.Moodlight.CurrentPreset = id;

                    foreach (IItem item in room.Items.Moodlights)
                    {
                        item.ExtraData = room.Moodlight.GenerateExtraData;
                        await room.SendPacketAsync(new WallItemUpdateComposer(item));
                    }
                }
                else
                {
                    preset.Enabled = false;
                }
            }

            await session.SendPacketAsync(new MoodLightDataComposer(id, room.Moodlight.Presets));
        }
    }
}
