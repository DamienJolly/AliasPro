using AliasPro.API.Catalog;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Catalog.Packets.Events
{
    public class RequestRecyclerLogicEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRecyclerLogicMessageEvent;

        private readonly ICatalogController _catalogController;

        public RequestRecyclerLogicEvent(ICatalogController catalogController)
        {
            _catalogController = catalogController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new RecyclerLogicComposer(
                _catalogController.GetRecyclerLevels, 
                _catalogController.GetRecyclerPrizes));
        }
    }
}
