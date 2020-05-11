using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;
using AliasPro.Game.Catalog.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Packets.Events
{
    internal class CatalogSearchedItemEvent : IMessageEvent
    {
        public short Header => Incoming.CatalogSearchedItemMessageEvent;

        private readonly CatalogController catalogController;

		public CatalogSearchedItemEvent(
			CatalogController catalogController)
        {
            this.catalogController = catalogController;
		}

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int offerId = message.ReadInt();

            if (offerId == -1)
                return;

            if (!catalogController.TryGetCatalogItemByOfferId(offerId, out CatalogItem catalogItem))
                return;

			await session.SendPacketAsync(new CatalogSearchResultComposer(catalogItem));
        }
	}
}
