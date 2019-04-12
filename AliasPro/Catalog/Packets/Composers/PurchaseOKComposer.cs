using AliasPro.API.Catalog.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Packets.Composers
{
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
