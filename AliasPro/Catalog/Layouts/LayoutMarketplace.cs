using AliasPro.API.Catalog.Layouts;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Models;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Layouts
{
    public class LayoutMarketplace : ICatalogLayout
    {
        private readonly ICatalogPage _page;

        public LayoutMarketplace(ICatalogPage page)
        {
            _page = page;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteString("marketplace");
            message.WriteInt(0);
            message.WriteInt(0);
		}

        public IItem HandleItemPurchase(ISession session, ICatalogItemData itemData, string extraData)
        {
            return new Item((uint)itemData.Id, session.Player.Id, session.Player.Username, "", itemData.ItemData);
        }
    }
}
