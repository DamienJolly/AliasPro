using AliasPro.API.Catalog.Layouts;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Models;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Layouts
{
    public class LayoutDefault : ICatalogLayout
    {
        private readonly ICatalogPage _page;

        public LayoutDefault(ICatalogPage page)
        {
            _page = page;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteString("default_3x3");
            message.WriteInt(3);
            message.WriteString(_page.HeaderImage);
            message.WriteString(_page.TeaserImage);
            message.WriteString(_page.SpecialImage);
            message.WriteInt(3);
            message.WriteString(_page.TextOne);
            message.WriteString(_page.TextDetails);
            message.WriteString(_page.TextTeaser);
        }

        public IItem HandlePurchase(ICatalogItemData catalogItem, ISession session, string extraData)
        {
            return new Item((uint)catalogItem.Id, session.Player.Id, "", catalogItem.ItemData);
        }
    }
}
