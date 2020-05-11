using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Game.Catalog.Purchase.Handlers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Purchase
{
	public class CatalogPurchaseHandler
	{
		private readonly Dictionary<string, IPurchaseHandler> handlers;

		public CatalogPurchaseHandler()
		{
			handlers = new Dictionary<string, IPurchaseHandler>();

			handlers.Add("default", new DefaultPurchaseHandler());
			handlers.Add("trophy", new TrophyPurchaseHandler());
			handlers.Add("badge_display", new BadgeDisplayPurchaseHandler());
			handlers.Add("teleport", new TeleportPurchaseHandler());

			//handlers.Add("pet", new PetPurchaseHandler());
			handlers.Add("bot", new BotPurchaseHandler());
		}

		public async Task<bool> TryHandlePurchase(ISession session, string extraData, IItemData itemData, int amount)
		{
			if (!handlers.TryGetValue(itemData.Interaction, out IPurchaseHandler handler))
			{
				if (!handlers.TryGetValue("default", out handler))
					return false;
			}

			return await handler.TryHandlePurchase(session, extraData, itemData, amount);
		}
	}
}
