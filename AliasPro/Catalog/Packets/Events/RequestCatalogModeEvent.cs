using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Sessions;

namespace AliasPro.Catalog.Packets.Events
{
    public class RequestCatalogModeEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestCatalogModeMessageEvent;

        private readonly ICatalogController _catalogController;

        public RequestCatalogModeEvent(ICatalogController catalogController)
        {
            _catalogController = catalogController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string mode = clientPacket.ReadString();
            await session.SendPacketAsync(new CatalogModeComposer(mode.Equals("normal") ? 0 : 1));
            await session.SendPacketAsync(new CatalogPagesListComposer(_catalogController, mode, session.Player.Rank));
        }
    }
}
