using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;
using System.Collections.Generic;

namespace AliasPro.Game.Catalog.Packets.Composers
{
    public class CatalogPagesListComposer : IMessageComposer
    {
        private readonly CatalogController catalogController;
        private readonly string mode;
        private readonly int rank;

        public CatalogPagesListComposer(CatalogController catalogController, string mode, int rank)
        {
            this.catalogController = catalogController;
            this.mode = mode;
            this.rank = rank;
        }

        public ServerMessage Compose()
        {
            ICollection<CatalogPage> pages = catalogController.GetCatalogPages(-1, rank);

            ServerMessage message = new ServerMessage(Outgoing.CatalogPagesListMessageComposer);
            message.WriteBoolean(true);
            message.WriteInt(0);
            message.WriteInt(-1);
            message.WriteString("root");
            message.WriteString("");

            message.WriteInt(0);

            message.WriteInt(pages.Count);
            foreach (CatalogPage page in pages)
                Append(message, page);

            message.WriteBoolean(false);
            message.WriteString(mode);
            return message;
        }
        
        private void Append(ServerMessage message, CatalogPage catalogPage)
        {
            ICollection<CatalogPage> pages = catalogController.GetCatalogPages(catalogPage.Id, rank);

            message.WriteBoolean(catalogPage.Visible);
            message.WriteInt(catalogPage.Icon);
            message.WriteInt(catalogPage.Enabled ? catalogPage.Id : -1);
            message.WriteString(catalogPage.Name);
            message.WriteString(catalogPage.Caption);

            message.WriteInt(catalogPage.OfferIds.Count);
            foreach (int offerId in catalogPage.OfferIds)
                message.WriteInt(offerId);

            message.WriteInt(pages.Count);
            foreach (CatalogPage page in pages)
                Append(message, page);
        }
    }
}
