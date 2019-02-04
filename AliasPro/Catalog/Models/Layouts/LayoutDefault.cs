namespace AliasPro.Catalog.Models.Layouts
{
    using Network.Protocol;

    public class LayoutDefault : ICatalogLayout
    {
        public void Compose(ServerPacket message, ICatalogPage page)
        {
            message.WriteString("default_3x3");
            message.WriteInt(3);
            message.WriteString(page.HeaderImage);
            message.WriteString(page.TeaserImage);
            message.WriteString(page.SpecialImage);
            message.WriteInt(3);
            message.WriteString(page.TextOne);
            message.WriteString(page.TextDetails);
            message.WriteString(page.TextTeaser);
        }
    }
}
