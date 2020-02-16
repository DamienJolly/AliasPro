using AliasPro.API.Catalog.Layouts;
using AliasPro.API.Catalog.Models;
using AliasPro.Catalog.Layouts;
using AliasPro.Catalog.Types;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Utilities;
using System.Collections.Generic;

namespace AliasPro.Catalog.Packets.Composers
{
    public class CatalogPageComposer : IMessageComposer
    {
        private readonly ICatalogPage _catalogPage;
        private readonly ICollection<ICatalogFeaturedPage> _featuredPages;
        private readonly string _mode;

        public CatalogPageComposer(
            ICatalogPage catalogPage, 
            ICollection<ICatalogFeaturedPage> featuredPages, 
            string mode)
        {
            _catalogPage = catalogPage;
            _featuredPages = featuredPages;
            _mode = mode;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.CatalogPageMessageComposer);
            message.WriteInt(_catalogPage.Id);
            message.WriteString(_mode);
            _catalogPage.Layout.Compose(message);

            message.WriteInt(_catalogPage.Items.Count);
            foreach (ICatalogItem catalogitem in _catalogPage.Items.Values)
            {
                catalogitem.Compose(message);
            }

            message.WriteInt(0);
            message.WriteBoolean(false);

            if (_catalogPage.Layout is LayoutFrontpage)
            {
                message.WriteInt(_featuredPages.Count);
                foreach (ICatalogFeaturedPage featuredPage in _featuredPages)
                {
                    message.WriteInt(featuredPage.SlotId);
                    message.WriteString(featuredPage.Caption);
                    message.WriteString(featuredPage.Image);
                    message.WriteInt((int)featuredPage.Type);
                    switch (featuredPage.Type)
                    {
                        case FeaturedPageType.PAGE_NAME:
                        default:
                            message.WriteString(featuredPage.PageName); break;
                        case FeaturedPageType.PAGE_ID:
                            message.WriteInt(featuredPage.PageId); break;
                        case FeaturedPageType.PRODUCT_NAME:
                            message.WriteString(featuredPage.ProductName); break;
                    }
                    message.WriteInt((int)UnixTimestamp.Now - featuredPage.ExpireTimestamp);
                }
            }

            return message;
        }
    }
}
