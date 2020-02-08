using AliasPro.API.Catalog;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Events
{
    public class RequestCatalogModeEvent : IMessageEvent
    {
        public short Header => Incoming.RequestCatalogModeMessageEvent;

        private readonly ICatalogController _catalogController;

        public RequestCatalogModeEvent(ICatalogController catalogController)
        {
            _catalogController = catalogController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            string mode = clientPacket.ReadString();
            await session.SendPacketAsync(new CatalogModeComposer(mode.Equals("normal") ? 0 : 1));
            await session.SendPacketAsync(new CatalogPagesListComposer(_catalogController, mode, session.Player.Rank));
        }
    }
}
