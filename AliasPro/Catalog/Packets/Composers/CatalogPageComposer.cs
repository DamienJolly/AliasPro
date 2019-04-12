using AliasPro.API.Network.Events;
using AliasPro.Catalog.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Packets.Composers
{
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

            message.WriteInt(_catalogPage.Items.Count);
            foreach (ICatalogItem catalogitem in _catalogPage.Items.Values)
            {
                catalogitem.Compose(message);
            }

            message.WriteInt(0);
            message.WriteBoolean(false);
            return message;
        }
    }
}
