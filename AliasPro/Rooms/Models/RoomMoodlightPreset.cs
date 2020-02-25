using AliasPro.API.Database;
using AliasPro.API.Rooms.Models;
using System.Data.Common;

namespace AliasPro.Rooms.Models
{
	public class RoomMoodlightPreset : IRoomMoodlightPreset
    {
        public RoomMoodlightPreset(DbDataReader reader)
        {
            Id = reader.ReadData<int>("preset_id");
            Enabled = reader.ReadData<bool>("enabled");
            BackgroundOnly = reader.ReadData<bool>("background_only");
            Colour = reader.ReadData<string>("colour");
            Intensity = reader.ReadData<int>("intensity");
        }

        public RoomMoodlightPreset(int id, bool enabled, bool backgroundOnly, string colour, int intensity)
        {
            Id = id;
            Enabled = enabled;
            BackgroundOnly = backgroundOnly;
            Colour = colour;
            Intensity = intensity;
        }

        public int Id { get; set; }
        public bool Enabled { get; set; }
        public bool BackgroundOnly { get; set; }
        public string Colour { get; set; }
        public int Intensity { get; set; }
    }
}
