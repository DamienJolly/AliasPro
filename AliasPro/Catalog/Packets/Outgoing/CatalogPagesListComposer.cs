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
        private readonly ICollection<ICatalogPage> _rootPages;
        private readonly string _mode;
        private readonly int _rank;

        public CatalogPagesListComposer(ICatalogController catalogController, ICollection<ICatalogPage> rootPages, string mode, int rank)
        {
            _catalogController = catalogController;
            _rootPages = rootPages;
            _mode = mode;
            _rank = rank;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.CatalogPagesListMessageComposer);
            message.WriteBoolean(true);
            message.WriteInt(0);
            message.WriteInt(-1);
            message.WriteString("root");
            message.WriteString("");

            message.WriteInt(0);

            message.WriteInt(_rootPages.Count);
            foreach (CatalogPage page in _rootPages)
            {
                Append(message, page);
            }
            message.WriteBoolean(false);
            message.WriteString(_mode);
            return message;
        }
        
        private async void Append(ServerPacket message, CatalogPage catalogPage)
        {
            ICollection<ICatalogPage> pages = await _catalogController.GetCatalogPagesAsync(catalogPage.Id, _rank);

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
