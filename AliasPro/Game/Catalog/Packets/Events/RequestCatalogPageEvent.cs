using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;
using AliasPro.Game.Catalog.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Packets.Events
{
    public class RequestCatalogPageEvent : IMessageEvent
    {
        public short Header => Incoming.RequestCatalogPageMessageEvent;

        private readonly CatalogController catalogController;

        public RequestCatalogPageEvent(CatalogController catalogController)
        {
            this.catalogController = catalogController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int catalogPageId = message.ReadInt();
            message.ReadInt(); // ??

            if (catalogController.TryGetCatalogPage(catalogPageId, out CatalogPage page))
            {
                string mode = message.ReadString();

                if (page.Rank <= session.Player.Rank && page.Enabled)
                    await session.SendPacketAsync(new CatalogPageComposer(page, catalogController.FeaturedPages, mode));
            }
        }
    }
}
