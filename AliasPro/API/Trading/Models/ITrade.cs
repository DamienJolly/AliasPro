using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Trading.Models
{
	public interface ITrade
	{
		bool TryGetPlayer(int playerId, out ITradePlayer player);
		bool TryAddPlayer(BaseEntity entity);
		Task EndTrade(bool confirmed, uint playerId = 0);
		Task SendPacketAsync(IMessageComposer packet);

		ICollection<ITradePlayer> Players { get; }
		bool Accepted { get; }
		bool Confirmed { get; }
	}
}
