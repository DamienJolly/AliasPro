using AliasPro.API.Catalog;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Events
{
    public class RequestRecyclerLogicEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RequestRecyclerLogicMessageEvent;

        private readonly ICatalogController _catalogController;

        public RequestRecyclerLogicEvent(ICatalogController catalogController)
        {
            _catalogController = catalogController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            await session.SendPacketAsync(new RecyclerLogicComposer(
                _catalogController.GetRecyclerLevels, 
                _catalogController.GetRecyclerPrizes));
        }
    }
}
