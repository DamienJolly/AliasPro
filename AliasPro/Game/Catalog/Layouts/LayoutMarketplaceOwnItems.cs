using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Layouts
{
    public class LayoutMarketplaceOwnItems : CatalogLayout
    {
        public LayoutMarketplaceOwnItems(CatalogPage page) :
            base(page)
        {

        }

        public override void ComposePageData(ServerMessage message)
        {
            message.WriteString("marketplace_own_items");
            message.WriteInt(0);
            message.WriteInt(0);
		}
    }
}
