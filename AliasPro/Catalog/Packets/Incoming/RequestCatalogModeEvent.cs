using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;

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
            await session.SendPacketAsync(new CatalogModeComposer(mode.Equals("normal") ? 0 : 1));
            await session.SendPacketAsync(new CatalogPagesListComposer(_catalogController, mode, session.Player.Rank));
        }
    }
}
