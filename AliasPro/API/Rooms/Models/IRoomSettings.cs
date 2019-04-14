namespace AliasPro.API.Rooms.Models
{
    public interface IRoomSettings
    {
        bool AllowPets { get; set; }
        bool AllowPetsEat { get; set; }
        bool RoomBlocking { get; set; }
        bool HideWalls { get; set; }
        int WallThickness { get; set; }
        int FloorThickness { get; set; }
        int WhoMutes { get; set; }
        int WhoKicks { get; set; }
        int WhoBans { get; set; }
        int ChatMode { get; set; }
        int ChatSize { get; set; }
        int ChatSpeed { get; set; }
        int ChatDistance { get; set; }
        int ChatFlood { get; set; }
    }
}
