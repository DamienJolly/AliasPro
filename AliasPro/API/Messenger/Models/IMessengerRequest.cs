using AliasPro.Network.Protocol;

namespace AliasPro.API.Messenger.Models
{
    public interface IMessengerRequest
    {
        void Compose(ServerPacket message);

        uint Id { get; }
        string Username { get; }
        string Figure { get; }
        string Motto { get; }
    }
}
