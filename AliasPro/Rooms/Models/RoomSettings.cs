using AliasPro.API.Database;
using AliasPro.API.Rooms.Models;
using System.Data.Common;

namespace AliasPro.Rooms.Models
{
    internal class RoomSettings : IRoomSettings
    {
        public RoomSettings(DbDataReader reader)
        {
            AllowPets = reader.ReadData<bool>("allow_pets");
            AllowPetsEat = reader.ReadData<bool>("allow_pets_eat");
            RoomBlocking = reader.ReadData<bool>("room_blocking");
            HideWalls = reader.ReadData<bool>("hide_walls");
            WallThickness = reader.ReadData<int>("wall_thickness");
            FloorThickness = reader.ReadData<int>("floor_thickness");
            WhoMutes = reader.ReadData<int>("who_mutes");
            WhoKicks = reader.ReadData<int>("who_kicks");
            WhoBans = reader.ReadData<int>("who_bans");
            ChatMode = reader.ReadData<int>("chat_mode");
            ChatSize = reader.ReadData<int>("chat_size");
            ChatSpeed = reader.ReadData<int>("chat_speed");
            ChatDistance = reader.ReadData<int>("chat_distance");
            ChatFlood = reader.ReadData<int>("chat_flood");
        }
        
        public bool AllowPets { get; set; }
        public bool AllowPetsEat { get; set; }
        public bool RoomBlocking { get; set; }
        public bool HideWalls { get; set; }
        public int WallThickness { get; set; }
        public int FloorThickness { get; set; }
        public int WhoMutes { get; set; }
        public int WhoKicks { get; set; }
        public int WhoBans { get; set; }
        public int ChatMode { get; set; }
        public int ChatSize { get; set; }
        public int ChatSpeed { get; set; }
        public int ChatDistance { get; set; }
        public int ChatFlood { get; set; }
    }
}
