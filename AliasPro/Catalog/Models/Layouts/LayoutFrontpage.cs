namespace AliasPro.Catalog.Models.Layouts
{
    using Network.Protocol;

    public class LayoutFrontpage : ICatalogLayout
    {
        public void Compose(ServerPacket message, ICatalogPage page)
        {
            message.WriteString("frontpage4");
            message.WriteInt(2);
            message.WriteString(page.HeaderImage);
            message.WriteString(page.TeaserImage);
            message.WriteInt(3);
            message.WriteString(page.TextOne);
            message.WriteString(page.TextDetails);
            message.WriteString(page.TextTeaser);
        }
    }
}
