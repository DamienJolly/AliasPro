using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Trading;
using AliasPro.API.Trading.Models;
using AliasPro.Rooms.Entities;
using AliasPro.Trading.Models;
using AliasPro.Trading.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Trading
{
	internal class TradingController : ITradingController
	{
		private readonly TradingDao _tradingDao;

		public TradingController(
			TradingDao tradingDao)
		{
			_tradingDao = tradingDao;
		}

		public ITrade StartTrade(BaseEntity playerOne, BaseEntity playerTwo)
		{
			ITrade trade = new Trade();

			trade.TryAddPlayer(playerOne);
			trade.TryAddPlayer(playerTwo);

			foreach (ITradePlayer player in trade.Players)
			{
				player.Entity.Actions.AddStatus("trd", "");
				player.Entity.Trade = trade;
			}

			return trade;
		}
	}
}
