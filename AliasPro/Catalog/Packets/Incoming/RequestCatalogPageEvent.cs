using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;

    public class RequestCatalogPageEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestCatalogPageMessageEvent;

        private readonly ICatalogController _catalogController;

        public RequestCatalogPageEvent(ICatalogController catalogController)
        {
            _catalogController = catalogController;
        }

        public async Task HandleAsync(
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
