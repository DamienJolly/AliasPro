using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Layouts
{
    public class LayoutBot : CatalogLayout
    {
        public LayoutBot(CatalogPage page) :
            base(page)
        {

        }

        public override void ComposePageData(ServerMessage message)
        {
            message.WriteString("bots");
            message.WriteInt(2);
            message.WriteString(Page.HeaderImage);
            message.WriteString(Page.TeaserImage);
            message.WriteInt(3);
            message.WriteString(Page.TextOne);
            message.WriteString(Page.TextDetails);
            message.WriteString(Page.TextTwo);
		}
    }
}
