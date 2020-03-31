using AliasPro.Players.Types;

namespace AliasPro.API.Players.Models
{
    public interface IPlayerSanction
    {
        SanctionType Type { get; set; }
        int StartTime { get; set; }
        int ExpireTime { get; set; }
        string Reason { get; set; }
        int TopicId { get; set; }
    }
}
