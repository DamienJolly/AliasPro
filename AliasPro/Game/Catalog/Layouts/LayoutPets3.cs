using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Layouts
{
    public class LayoutPets3 : CatalogLayout
    {
        public LayoutPets3(CatalogPage page) :
            base(page)
        {

        }

        public override void ComposePageData(ServerMessage message)
        {
            message.WriteString("pets3");
            message.WriteInt(2);
            message.WriteString(Page.HeaderImage);
            message.WriteString(Page.TeaserImage);
            message.WriteInt(4);
            message.WriteString(Page.TextOne);
            message.WriteString(Page.TextTwo);
            message.WriteString(Page.TextDetails);
            message.WriteString(Page.TextTeaser);
		}
    }
}
