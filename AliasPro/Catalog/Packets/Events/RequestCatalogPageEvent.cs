using AliasPro.API.Catalog;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Sessions;

namespace AliasPro.Catalog.Packets.Events
{
    public class RequestCatalogPageEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestCatalogPageMessageEvent;

        private readonly ICatalogController _catalogController;

        public RequestCatalogPageEvent(ICatalogController catalogController)
        {
            _catalogController = catalogController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int catalogPageId = clientPacket.ReadInt();
            clientPacket.ReadInt(); // ??

            if (_catalogController.TryGetCatalogPage(catalogPageId, out ICatalogPage page))
            {
                string mode = clientPacket.ReadString();

                if (page.Rank <= session.Player.Rank && page.Enabled)
                    await session.SendPacketAsync(new CatalogPageComposer(page, mode));
            }
        }
    }
}
