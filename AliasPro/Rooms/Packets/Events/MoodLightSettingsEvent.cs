using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Components;
using AliasPro.Rooms.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class MoodLightSettingsEvent : IMessageEvent
    {
        public short Header => Incoming.MoodLightSettingsMessageEvent;

        private readonly IRoomController _roomController;

        public MoodLightSettingsEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null)
                return;

            if (room.Moodlight == null)
            {
                IDictionary<int, IRoomMoodlightPreset> presets = 
                    await _roomController.GetMoodlightPresets(room.Id);

                if (presets.Count < 3)
                {
                    for (int i = presets.Count + 1; i <= 3; i++)
                    {
                        IRoomMoodlightPreset preset = 
                            new RoomMoodlightPreset(i, false, false, "#000000", 255);

                        presets.TryAdd(i, preset);
                        await _roomController.AddMoodlightPreset(room.Id, preset);
                    }
                }

                room.Moodlight = 
                    new MoodlightComponent(room, presets);
            }

            await session.SendPacketAsync(new MoodLightDataComposer(
                room.Moodlight.CurrentPreset,
                room.Moodlight.Presets));
        }
    }
}
