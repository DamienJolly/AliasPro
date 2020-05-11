using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Packets.Events
{
    public class RequestCatalogModeEvent : IMessageEvent
    {
        public short Header => Incoming.RequestCatalogModeMessageEvent;

        private readonly CatalogController catalogController;

        public RequestCatalogModeEvent(CatalogController catalogController)
        {
            this.catalogController = catalogController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            string mode = message.ReadString();
            await session.SendPacketAsync(new CatalogModeComposer(mode.Equals("normal") ? 0 : 1));
            await session.SendPacketAsync(new CatalogPagesListComposer(catalogController, mode, session.Player.Rank));
        }
    }
}
