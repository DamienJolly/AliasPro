using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Layouts
{
    public class LayoutRecycler : CatalogLayout
    {
        public LayoutRecycler(CatalogPage page) :
            base(page)
        {

        }

        public override void ComposePageData(ServerMessage message)
        {
            message.WriteString("recycler");
            message.WriteInt(2);
            message.WriteString(Page.HeaderImage);
            message.WriteString(Page.TeaserImage);
            message.WriteInt(1);
            message.WriteString(Page.TextOne);
            message.WriteInt(-1);
            message.WriteBoolean(false);
        }
    }
}
