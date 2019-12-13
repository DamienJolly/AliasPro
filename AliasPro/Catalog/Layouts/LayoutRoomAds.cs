using AliasPro.API.Catalog.Layouts;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Models;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Layouts
{
    public class LayoutRoomAds : ICatalogLayout
    {
        private readonly ICatalogPage _page;

        public LayoutRoomAds(ICatalogPage page)
        {
            _page = page;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteString("roomads");
            message.WriteInt(2);
            message.WriteString(_page.HeaderImage);
            message.WriteString(_page.TeaserImage);
            message.WriteInt(2);
            message.WriteString(_page.TextOne);
            message.WriteString(_page.TextTwo);
        }

        public IItem HandleItemPurchase(ISession session, ICatalogItemData itemData, string extraData)
        {
            return new Item((uint)itemData.Id, session.Player.Id, "", itemData.ItemData);
        }
    }
}
