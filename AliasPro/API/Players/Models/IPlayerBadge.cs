namespace AliasPro.API.Players.Models
{
    public interface IPlayerBadge
    {
        int BadgeId { get; set; }
        string Code { get; set; }
        int Slot { get; set; }
    }
}
