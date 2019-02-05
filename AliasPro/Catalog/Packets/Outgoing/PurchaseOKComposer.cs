namespace AliasPro.Catalog.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class PurchaseOKComposer : IPacketComposer
    {
        private readonly ICatalogItem _item;

        public PurchaseOKComposer(ICatalogItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.PurchaseOKMessageComposer);
            _item.Compose(message);
            return message;
        }
    }
}
