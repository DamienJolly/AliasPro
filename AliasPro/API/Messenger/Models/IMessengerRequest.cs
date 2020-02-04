using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.API.Messenger.Models
{
    public interface IMessengerRequest
    {
        void Compose(ServerMessage message);

        uint Id { get; }
        string Username { get; }
        string Figure { get; }
        string Motto { get; }
    }
}
