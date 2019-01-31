namespace AliasPro.Room.Models
{
    public interface IRoom
    {
        IRoomData RoomData { get; set; }
        IRoomModel RoomModel { get; set; }
    }
}
