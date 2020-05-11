using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Layouts
{
    public class LayoutDefault : CatalogLayout
    {
        public LayoutDefault(CatalogPage page) :
            base(page)
        {

        }

        public override void ComposePageData(ServerMessage message)
        {
            message.WriteString("default_3x3");
            message.WriteInt(3);
            message.WriteString(Page.HeaderImage);
            message.WriteString(Page.TeaserImage);
            message.WriteString(Page.SpecialImage);
            message.WriteInt(3);
            message.WriteString(Page.TextOne);
            message.WriteString(Page.TextDetails);
            message.WriteString(Page.TextTeaser);
        }
    }
}
