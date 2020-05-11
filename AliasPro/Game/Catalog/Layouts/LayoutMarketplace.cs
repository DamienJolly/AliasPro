
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Layouts
{
    public class LayoutMarketplace : CatalogLayout
    {
        public LayoutMarketplace(CatalogPage page) :
            base(page)
        {

        }

        public override void ComposePageData(ServerMessage message)
        {
            message.WriteString("marketplace");
            message.WriteInt(0);
            message.WriteInt(0);
		}
    }
}
