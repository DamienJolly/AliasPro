using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Trading.Models
{
	public interface ITrade
	{
		ICollection<ITradePlayer> Players { get; }
		bool TryGetPlayer(int playerId, out ITradePlayer player);
		bool TryAddPlayer(BaseEntity entity);
		Task SendAsync(IPacketComposer packet);
	}
}
