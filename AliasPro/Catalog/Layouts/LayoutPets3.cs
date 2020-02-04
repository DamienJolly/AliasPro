using AliasPro.API.Catalog.Layouts;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Models;

namespace AliasPro.Catalog.Layouts
{
    public class LayoutPets3 : ICatalogLayout
    {
        private readonly ICatalogPage _page;

        public LayoutPets3(ICatalogPage page)
        {
            _page = page;
        }

        public void Compose(ServerMessage message)
        {
            message.WriteString("pets3");
            message.WriteInt(2);
            message.WriteString(_page.HeaderImage);
            message.WriteString(_page.TeaserImage);
            message.WriteInt(4);
            message.WriteString(_page.TextOne);
            message.WriteString(_page.TextTwo);
            message.WriteString(_page.TextDetails);
            message.WriteString(_page.TextTeaser);
		}

        public IItem HandleItemPurchase(ISession session, ICatalogItemData itemData, string extraData)
        {
            return new Item((uint)itemData.Id, session.Player.Id, session.Player.Username, "", itemData.ItemData);
        }
    }
}
