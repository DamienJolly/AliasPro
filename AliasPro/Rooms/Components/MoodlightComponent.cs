using AliasPro.API.Rooms.Models;
using System.Collections.Generic;

namespace AliasPro.Rooms.Components
{
    public class MoodlightComponent
    {
        private readonly IRoom _room;
        private readonly IDictionary<int, IRoomMoodlightPreset> _moodlightPresets;

        public bool Enabled = false;
        public int CurrentPreset = 1;

        public MoodlightComponent(
            IRoom room,
            IDictionary<int, IRoomMoodlightPreset> moodlightPresets)
        {
            _room = room;
            _moodlightPresets = moodlightPresets;

            foreach (IRoomMoodlightPreset preset in _moodlightPresets.Values)
            {
                if (preset.Enabled)
                {
                    Enabled = true;
                    CurrentPreset = preset.Id;
                    break;
                }
            }
        }

        public string GenerateExtraData =>
            !TryGetPreset(CurrentPreset, out IRoomMoodlightPreset preset) ? "" 
            : (Enabled ? 2 : 1) + "," + preset.Id + "," + (preset.BackgroundOnly ? 2 : 1) + "," + preset.Colour + "," + preset.Intensity;

        public ICollection<IRoomMoodlightPreset> Presets =>
            _moodlightPresets.Values;

        public bool TryGetPreset(int presetId, out IRoomMoodlightPreset preset) => 
            _moodlightPresets.TryGetValue(presetId, out preset);
    }
}
