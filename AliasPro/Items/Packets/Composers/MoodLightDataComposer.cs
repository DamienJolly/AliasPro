using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Items.Packets.Composers
{
    public class MoodLightDataComposer : IMessageComposer
    {
        private readonly int _currentPreset;
        private readonly ICollection<IRoomMoodlightPreset> _moodlightPresets;

		public MoodLightDataComposer(
            int currentPreset,
            ICollection<IRoomMoodlightPreset> moodlightPresets)
        {
            _currentPreset = currentPreset;
            _moodlightPresets = moodlightPresets;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.MoodLightDataMessageComposer);
            message.WriteInt(_moodlightPresets.Count);
            message.WriteInt(_currentPreset);

            foreach (IRoomMoodlightPreset data in _moodlightPresets)
            {
                message.WriteInt(data.Id);
                message.WriteInt(data.BackgroundOnly ? 2 : 1);
                message.WriteString(data.Colour);
                message.WriteInt(data.Intensity);
            }

            return message;
        }
    }
}
