using AliasPro.Network.Protocol;

namespace AliasPro.API.Chat.Models
{
    public interface IChatLog
    {
        void Compose(ServerPacket message);

        int PlayerId { get; set; }
        string PlayerUsername { get; set; }
        int Timestamp { get; set; }
        string Message { get; set; }
    }
}
