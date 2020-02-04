using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.API.Players.Models
{
    public interface IPlayerRoomVisited
    {
        void Compose(ServerMessage message);

        int RoomId { get; set; }
        string RoomName { get; set; }
        int EntryTimestamp { get; set; }
        int ExitTimestamp { get; set; }
    }
}
