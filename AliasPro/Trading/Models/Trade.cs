using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Trading.Models;
using AliasPro.Rooms.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Trading.Models
{
	internal class Trade : ITrade
	{
		private readonly IDictionary<int, ITradePlayer> _players;

		internal Trade()
		{
			_players = new Dictionary<int, ITradePlayer>();
		}

		public bool TryGetPlayer(int playerId, out ITradePlayer player) =>
			_players.TryGetValue(playerId, out player);

		public bool TryAddPlayer(BaseEntity entity) =>
			_players.TryAdd(entity.Id, new TradePlayer(entity));

		public async Task SendAsync(IPacketComposer packet)
		{
			foreach (ITradePlayer player in _players.Values)
			{
				if (player.Entity is PlayerEntity playerEntity)
				{
					if (playerEntity.Session != null)
						await playerEntity.Session.SendPacketAsync(packet);
				}
			}
		}

		public ICollection<ITradePlayer> Players =>
			_players.Values;

		public bool Accepted =>
			Players.Where(tradeUser => tradeUser.Accepted).Count() >= Players.Count;

		public bool Confirmed =>
			Players.Where(tradeUser => tradeUser.Confirmed).Count() >= Players.Count;
	}
}
