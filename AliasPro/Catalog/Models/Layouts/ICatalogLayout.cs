namespace AliasPro.Catalog.Models.Layouts
{
    using Network.Protocol;

    public interface ICatalogLayout
    {
        void Compose(ServerPacket message, ICatalogPage page);
    }
}
