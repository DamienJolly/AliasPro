namespace AliasPro.Catalog.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class CatalogPageComposer : IPacketComposer
    {
        private readonly ICatalogPage _catalogPage;
        private readonly string _mode;

        public CatalogPageComposer(ICatalogPage catalogPage, string mode)
        {
            _catalogPage = catalogPage;
            _mode = mode;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.CatalogPageMessageComposer);
            message.WriteInt(_catalogPage.Id);
            message.WriteString(_mode);
            _catalogPage.Layout.Compose(message, _catalogPage);

            message.WriteInt(0);

            message.WriteInt(0);
            message.WriteBoolean(false);
            return message;
        }
    }
}
