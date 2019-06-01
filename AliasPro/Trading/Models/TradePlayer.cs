using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Trading.Models;
using AliasPro.Rooms.Entities;
using System.Collections.Generic;

namespace AliasPro.Trading.Models
{
	internal class TradePlayer : ITradePlayer
	{
		public uint playerId { get; set; } = 0;
		public BaseEntity Entity { get; set; }
		public IDictionary<uint, IItem> OfferedItems { get; set; }
		public bool Accepted { get; set; } = false;
		public bool Confirmed { get; set; } = false;

		internal TradePlayer(BaseEntity entity)
		{
			Entity = entity;

			if (entity is PlayerEntity playerEntity)
				playerId = playerEntity.Player.Id;

			OfferedItems = new Dictionary<uint, IItem>();
		}

		public bool TryAddItem(IItem item) =>
			OfferedItems.TryAdd(item.Id, item);

		public void RemoveItem(uint itemId) =>
			OfferedItems.Remove(itemId);
	}
}
