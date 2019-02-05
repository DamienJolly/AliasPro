namespace AliasPro.Catalog.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

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
