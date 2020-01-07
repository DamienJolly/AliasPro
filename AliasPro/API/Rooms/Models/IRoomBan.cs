namespace AliasPro.API.Rooms.Models
{
    public interface IRoomBan
    {
        int PlayerId { get; set; }
        string PlayerName { get; set; }
        int ExpireTime { get; set; }
        bool HasExpired { get; }
    }
}
