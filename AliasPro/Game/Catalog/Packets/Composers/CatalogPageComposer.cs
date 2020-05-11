using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Layouts;
using AliasPro.Game.Catalog.Models;
using System.Collections.Generic;

namespace AliasPro.Game.Catalog.Packets.Composers
{
    public class CatalogPageComposer : IMessageComposer
    {
        private readonly CatalogPage catalogPage;
        private readonly ICollection<CatalogFeaturedPage> featuredPages;
        private readonly string mode;

        public CatalogPageComposer(
            CatalogPage catalogPage, 
            ICollection<CatalogFeaturedPage> featuredPages, 
            string mode)
        {
            this.catalogPage = catalogPage;
            this.featuredPages = featuredPages;
            this.mode = mode;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.CatalogPageMessageComposer);
            message.WriteInt(catalogPage.Id);
            message.WriteString(mode);
            catalogPage.Layout.ComposePageData(message);

            message.WriteInt(catalogPage.Items.Count);
            foreach (CatalogItem catalogitem in catalogPage.Items.Values)
            {
                catalogitem.Compose(message);
            }

            message.WriteInt(0);
            message.WriteBoolean(false);

            if (catalogPage.Layout is LayoutFrontpage)
            {
                message.WriteInt(featuredPages.Count);
                foreach (CatalogFeaturedPage featuredPage in featuredPages)
                {
                    featuredPage.Compose(message);
                }
            }

            return message;
        }
    }
}
