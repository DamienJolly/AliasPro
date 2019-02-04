using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Incoming
{
    using AliasPro.Catalog.Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using System.Collections.Generic;

    public class RequestCatalogModeEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestCatalogModeMessageEvent;

        private readonly ICatalogController _catalogController;

        public RequestCatalogModeEvent(ICatalogController catalogController)
        {
            _catalogController = catalogController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string mode = clientPacket.ReadString();
            ICollection<ICatalogPage> rootPages = _catalogController.GetCatalogPages(-1, session.Player.Rank);

            await session.SendPacketAsync(new CatalogModeComposer(mode.Equals("normal") ? 0 : 1));
            await session.SendPacketAsync(new CatalogPagesListComposer(_catalogController, rootPages, mode, session.Player.Rank));
        }
    }
}
