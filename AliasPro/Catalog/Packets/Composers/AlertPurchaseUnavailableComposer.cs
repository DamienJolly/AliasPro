using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Packets.Composers
{
    public class AlertPurchaseUnavailableComposer : IPacketComposer
    {
        public static int ILLEGAL = 0;
        public static int REQUIRES_CLUB = 1;

        private readonly int _code;

        public AlertPurchaseUnavailableComposer(int code)
        {
            _code = code;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AlertPurchaseUnavailableMessageComposer);
            message.WriteInt(_code);
            return message;
        }
    }
}
