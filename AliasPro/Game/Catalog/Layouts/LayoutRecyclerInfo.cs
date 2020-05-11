using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Layouts
{
    public class LayoutRecyclerInfo : CatalogLayout
    {
        public LayoutRecyclerInfo(CatalogPage page) :
            base(page)
        {

        }

        public override void ComposePageData(ServerMessage message)
        {
            message.WriteString("recycler_info");
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
