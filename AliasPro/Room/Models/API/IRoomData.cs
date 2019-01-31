namespace AliasPro.Room.Models
{
    public interface IRoomData
    {
        uint Id { get; set; }
        int Score { get; set; }
        int OwnerId { get; set; }
        string Name { get; set; }
        string Password { get; set; }
        string ModelName { get; set; }
    }
}
