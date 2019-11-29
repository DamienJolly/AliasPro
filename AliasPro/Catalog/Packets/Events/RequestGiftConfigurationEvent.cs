using AliasPro.API.Catalog;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Catalog.Packets.Events
{
    public class RequestGiftConfigurationEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestGiftConfigurationMessageEvent;

        private readonly ICatalogController _catalogController;

        public RequestGiftConfigurationEvent(ICatalogController catalogController)
        {
            _catalogController = catalogController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new GiftConfigurationComposer(
				_catalogController.GetGifts,
				_catalogController.GetWrappers));
        }
    }
}
