using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Trading.Models;
using System.Collections.Generic;

namespace AliasPro.Trading.Models
{
	internal class TradePlayer : ITradePlayer
	{
		public BaseEntity Entity { get; set; }
		public IDictionary<uint, IItem> OfferedItems { get; set; }

		internal TradePlayer(BaseEntity entity)
		{
			Entity = entity;
			OfferedItems = new Dictionary<uint, IItem>();
		}

		public bool TryAddItem(IItem item) =>
			OfferedItems.TryAdd(item.Id, item);
	}
}
