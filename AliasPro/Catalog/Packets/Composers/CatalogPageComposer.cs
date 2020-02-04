using AliasPro.API.Catalog.Layouts;
using AliasPro.API.Catalog.Models;
using AliasPro.Catalog.Layouts;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Catalog.Packets.Composers
{
    public class CatalogPageComposer : IMessageComposer
    {
        private readonly ICatalogPage _catalogPage;
        private readonly string _mode;

        public CatalogPageComposer(ICatalogPage catalogPage, string mode)
        {
            _catalogPage = catalogPage;
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
                message.WriteInt(0); //count
                {
                    //message.WriteInt(SlotId);
                    //message.WriteString(Caption);
                    //message.WriteString(Image);
                    //message.WriteInt(Type);
                    /*switch (Type)
                    {
                        case PAGE_NAME:
                            message.WriteString(PageName);
                            break;
                        case PAGE_ID:
                            message.WriteInt(PageId);
                            break;
                        case PRODUCT_NAME:
                            message.WriteString(ProductName);
                            break;
                    }*/
                    //message.WriteInt(-1); //Expire Time
                }
            }

            return message;
        }
    }
}
