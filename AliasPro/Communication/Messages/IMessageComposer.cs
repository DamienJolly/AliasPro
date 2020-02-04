using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Communication.Messages
{
    public interface IMessageComposer
    {
        ServerMessage Compose();
    }
}
