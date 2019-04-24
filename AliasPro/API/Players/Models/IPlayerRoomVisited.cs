using AliasPro.Network.Protocol;

namespace AliasPro.API.Players.Models
{
    public interface IPlayerRoomVisited
    {
        void Compose(ServerPacket message);

        int RoomId { get; set; }
        string RoomName { get; set; }
        int EntryTimestamp { get; set; }
        int ExitTimestamp { get; set; }
    }
}
