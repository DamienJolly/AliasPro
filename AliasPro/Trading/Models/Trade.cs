using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Trading.Models;
using AliasPro.Rooms.Entities;
using System.Collections.Generic;
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

		public ICollection<ITradePlayer> Players =>
			_players.Values;

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
	}
}
