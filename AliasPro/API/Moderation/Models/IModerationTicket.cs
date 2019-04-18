using AliasPro.Moderation.Types;
using AliasPro.Network.Protocol;

namespace AliasPro.API.Moderation.Models
{
    public interface IModerationTicket
    {
        void Compose(ServerPacket serverPacket);

        int Id { get; set; }
        ModerationTicketState State { get; set; }
        ModerationTicketType Type { get; set; }
        int Category { get; set; }
        int Timestamp { get; set; }
        int Priority { get; set; }
        int SenderId { get; set; }
        string SenderUsername { get; set; }
        int ReportedId { get; set; }
        string ReportedUsername { get; set; }
        int ModId { get; set; }
        string ModUsername { get; set; }
        string Caption { get; set; }
        int RoomId { get; set; }
    }
}
