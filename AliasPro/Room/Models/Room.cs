namespace AliasPro.Room.Models
{
    internal class Room : IRoom
    {
        internal Room(IRoomData roomData, IRoomModel model)
        {
            RoomData = roomData;
            RoomModel = model;
        }
        
        public IRoomData RoomData { get; set; }
        public IRoomModel RoomModel { get; set; }
    }
}
