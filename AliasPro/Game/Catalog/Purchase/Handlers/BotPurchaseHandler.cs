using AliasPro.API.Items.Models;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Game.Catalog.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Players.Models;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Purchase.Handlers
{
	public class BotPurchaseHandler : IPurchaseHandler
	{
		public async Task<bool> TryHandlePurchase(ISession session, string extraData, IItemData itemData, int amount)
		{
			int.TryParse(extraData, out int botId);
			if (!Program.GetService<CatalogController>().TryGetBotData(botId, out CatalogBotData botData))
				return false;

			IPlayerBot playerBot = new PlayerBot(0, botData.Name, botData.Motto, botData.Gender, botData.Figure);
			playerBot.Id = await Program.GetService<CatalogDao>().AddNewBotAsync(playerBot, (int)session.Player.Id);
			session.Player.Inventory.TryAddBot(playerBot);

			await session.SendPacketAsync(new AddPlayerItemsComposer(5, playerBot.Id));
			return true;
		}
	}
}
