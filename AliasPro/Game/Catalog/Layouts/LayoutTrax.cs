using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Layouts
{
    public class LayoutTrax : CatalogLayout
    {
        public LayoutTrax(CatalogPage page) :
            base(page)
        {

        }

        public override void ComposePageData(ServerMessage message)
        {
            message.WriteString("soundmachine");
            message.WriteInt(2);
            message.WriteString(Page.HeaderImage);
            message.WriteString(Page.TeaserImage);
            message.WriteInt(2);
            message.WriteString(Page.TextOne);
            message.WriteString(Page.TextDetails);
        }
    }
}
