namespace AliasPro.API.Rooms.Models
{
    public interface IRoomPromotion
    {
        int Id { get; set; }
        int Category { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        int StartTimestamp { get; set; }
        int EndTimestamp { get; set; }
    }
}
