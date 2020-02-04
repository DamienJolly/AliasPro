using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Catalog.Packets.Composers
{
    public class AlertPurchaseFailedComposer : IMessageComposer
    {
        public static int SERVER_ERROR = 0;
        public static int ALREADY_HAVE_BADGE = 1;

        private readonly int _error;

        public AlertPurchaseFailedComposer(int error)
        {
            _error = error;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AlertPurchaseFailedMessageComposer);
            message.WriteInt(_error);
            return message;
        }
    }
}
