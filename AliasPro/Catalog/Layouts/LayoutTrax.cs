using AliasPro.API.Catalog.Layouts;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Models;

namespace AliasPro.Catalog.Layouts
{
    public class LayoutTrax : ICatalogLayout
    {
        private readonly ICatalogPage _page;

        public LayoutTrax(ICatalogPage page)
        {
            _page = page;
        }

        public void Compose(ServerMessage message)
        {
            message.WriteString("soundmachine");
            message.WriteInt(2);
            message.WriteString(_page.HeaderImage);
            message.WriteString(_page.TeaserImage);
            message.WriteInt(2);
            message.WriteString(_page.TextOne);
            message.WriteString(_page.TextDetails);
        }

        public IItem HandleItemPurchase(ISession session, ICatalogItemData itemData, string extraData)
        {
            return new Item((uint)itemData.Id, session.Player.Id, session.Player.Username, "", itemData.ItemData);
        }
    }
}
