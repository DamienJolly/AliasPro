using AliasPro.Communication.Messages.Protocols;
using AliasPro.Moderation.Types;

namespace AliasPro.API.Moderation.Models
{
    public interface IModerationTicket
    {
        void Compose(ServerMessage ServerMessage);

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
