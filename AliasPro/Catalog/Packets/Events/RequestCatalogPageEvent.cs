using AliasPro.API.Catalog;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Events
{
    public class RequestCatalogPageEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RequestCatalogPageMessageEvent;

        private readonly ICatalogController _catalogController;

        public RequestCatalogPageEvent(ICatalogController catalogController)
        {
            _catalogController = catalogController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
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
