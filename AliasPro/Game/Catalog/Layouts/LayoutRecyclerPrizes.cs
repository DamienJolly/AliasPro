using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Layouts
{
    public class LayoutRecyclerPrizes : CatalogLayout
    {
        public LayoutRecyclerPrizes(CatalogPage page) :
            base(page)
        {

        }

        public override void ComposePageData(ServerMessage message)
        {
            message.WriteString("recycler_prizes");
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
