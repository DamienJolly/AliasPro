using AliasPro.API.Catalog.Layouts;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Models;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Layouts
{
    public class LayoutGroup : ICatalogLayout
    {
        private readonly ICatalogPage _page;

        public LayoutGroup(ICatalogPage page)
        {
            _page = page;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteString("guild_frontpage");
            message.WriteInt(2);
            message.WriteString(_page.HeaderImage);
            message.WriteString(_page.TeaserImage);
            message.WriteInt(3);
            message.WriteString(_page.TextOne);
            message.WriteString(_page.TextDetails);
            message.WriteString(_page.TextTeaser);
        }

        public IItem HandleItemPurchase(ISession session, ICatalogItemData itemData, string extraData)
        {
            return new Item((uint)itemData.Id, session.Player.Id, "", itemData.ItemData);
        }
    }
}
