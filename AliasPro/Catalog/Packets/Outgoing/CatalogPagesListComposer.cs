using System.Collections.Generic;

namespace AliasPro.Catalog.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class CatalogPagesListComposer : IPacketComposer
    {
        private readonly ICatalogController _catalogController;
        private readonly string _mode;
        private readonly int _rank;

        public CatalogPagesListComposer(ICatalogController catalogController, string mode, int rank)
        {
            _catalogController = catalogController;
            _mode = mode;
            _rank = rank;
        }

        public ServerPacket Compose()
        {
            ICollection<ICatalogPage> pages = _catalogController.GetCatalogPages(-1, _rank);

            ServerPacket message = new ServerPacket(Outgoing.CatalogPagesListMessageComposer);
            message.WriteBoolean(true);
            message.WriteInt(0);
            message.WriteInt(-1);
            message.WriteString("root");
            message.WriteString("");

            message.WriteInt(0);

            message.WriteInt(pages.Count);
            foreach (CatalogPage page in pages)
            {
                Append(message, page);
            }
            message.WriteBoolean(false);
            message.WriteString(_mode);
            return message;
        }
        
        private void Append(ServerPacket message, CatalogPage catalogPage)
        {
            ICollection<ICatalogPage> pages = _catalogController.GetCatalogPages(catalogPage.Id, _rank);

            message.WriteBoolean(catalogPage.Visible);
            message.WriteInt(catalogPage.Icon);
            message.WriteInt(catalogPage.Enabled ? catalogPage.Id : -1);
            message.WriteString(catalogPage.Name);
            message.WriteString(catalogPage.Caption);

            message.WriteInt(0);

            message.WriteInt(pages.Count);
            foreach (CatalogPage page in pages)
            {
                Append(message, page);
            }
        }
    }
}
