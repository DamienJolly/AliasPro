namespace AliasPro.API.Rooms.Models
{
    public interface IRoomMoodlightPreset
    {
        int Id { get; set; }
        bool Enabled { get; set; }
        bool BackgroundOnly { get; set; }
        string Colour { get; set; }
        int Intensity { get; set; }
    }
}
