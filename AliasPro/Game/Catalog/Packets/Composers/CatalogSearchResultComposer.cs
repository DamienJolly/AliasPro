using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Packets.Composers
{
    public class CatalogSearchResultComposer : IMessageComposer
    {
        private readonly CatalogItem catalogItem;

        public CatalogSearchResultComposer(CatalogItem catalogItem)
        {
            this.catalogItem = catalogItem;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.CatalogSearchResultMessageComposer);
            catalogItem.Compose(message);
            return message;
        }
    }
}
