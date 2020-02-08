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
        public short Header => Incoming.RequestCatalogPageMessageEvent;

        private readonly ICatalogController _catalogController;

        public RequestCatalogPageEvent(ICatalogController catalogController)
        {
            _catalogController = catalogController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int catalogPageId = message.ReadInt();
            message.ReadInt(); // ??

            if (_catalogController.TryGetCatalogPage(catalogPageId, out ICatalogPage page))
            {
                string mode = message.ReadString();

                if (page.Rank <= session.Player.Rank && page.Enabled)
                    await session.SendPacketAsync(new CatalogPageComposer(page, mode));
            }
        }
    }
}
