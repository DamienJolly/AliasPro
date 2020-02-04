using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Players.Packets.Composers
{
    public class AvailabilityStatusComposer : IMessageComposer
    {
        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AvailabilityStatusMessageComposer);
            message.WriteBoolean(true);
            message.WriteBoolean(false);
            message.WriteBoolean(true);
            return message;
        }
    }
}
