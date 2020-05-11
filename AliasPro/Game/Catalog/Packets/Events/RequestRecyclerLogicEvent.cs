using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Packets.Events
{
    public class RequestRecyclerLogicEvent : IMessageEvent
    {
        public short Header => Incoming.RequestRecyclerLogicMessageEvent;

        private readonly CatalogController catalogController;

        public RequestRecyclerLogicEvent(CatalogController catalogController)
        {
            this.catalogController = catalogController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            await session.SendPacketAsync(new RecyclerLogicComposer(
                catalogController.GetRecyclerLevels,
                catalogController.GetRecyclerPrizes));
        }
    }
}
