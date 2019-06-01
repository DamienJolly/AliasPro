using AliasPro.API.Rooms.Entities;
using AliasPro.API.Trading.Models;

namespace AliasPro.Trading.Models
{
	internal class TradePlayer : ITradePlayer
	{
		public BaseEntity Entity { get; set; }

		internal TradePlayer(BaseEntity entity)
		{
			Entity = entity;
		}
	}
}
