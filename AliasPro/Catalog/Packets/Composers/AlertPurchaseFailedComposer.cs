using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Packets.Composers
{
    public class AlertPurchaseFailedComposer : IPacketComposer
    {
        public static int SERVER_ERROR = 0;
        public static int ALREADY_HAVE_BADGE = 1;

        private readonly int _error;

        public AlertPurchaseFailedComposer(int error)
        {
            _error = error;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AlertPurchaseFailedMessageComposer);
            message.WriteInt(_error);
            return message;
        }
    }
}
