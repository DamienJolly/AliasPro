using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Catalog.Packets.Composers
{
    public class AlertPurchaseUnavailableComposer : IMessageComposer
    {
        public static int ILLEGAL = 0;
        public static int REQUIRES_CLUB = 1;

        private readonly int _code;

        public AlertPurchaseUnavailableComposer(int code)
        {
            _code = code;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AlertPurchaseUnavailableMessageComposer);
            message.WriteInt(_code);
            return message;
        }
    }
}
