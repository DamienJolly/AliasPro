using AliasPro.API.Catalog.Layouts;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Items.Models;
using AliasPro.Network.Protocol;
using AliasPro.Sessions;

namespace AliasPro.Catalog.Layouts
{
    public class LayoutFrontpage : ICatalogLayout
    {
        private readonly ICatalogPage _page;

        public LayoutFrontpage(ICatalogPage page)
        {
            _page = page;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteString("frontpage4");
            message.WriteInt(2);
            message.WriteString(_page.HeaderImage);
            message.WriteString(_page.TeaserImage);
            message.WriteInt(3);
            message.WriteString(_page.TextOne);
            message.WriteString(_page.TextDetails);
            message.WriteString(_page.TextTeaser);
        }

        public IItem HandlePurchase(ICatalogItemData catalogItem, ISession session, string extraData)
        {
            return null;
        }
    }
}
