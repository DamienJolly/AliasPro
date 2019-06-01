using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using System.Collections.Generic;

namespace AliasPro.API.Trading.Models
{
	public interface ITradePlayer
	{
		uint playerId { get; set; }
		BaseEntity Entity { get; set; }
		IDictionary<uint, IItem> OfferedItems { get; set; }
		bool Accepted { get; set; }
		bool Confirmed { get; set; }

		bool TryAddItem(IItem item);
		void RemoveItem(uint itemId);
	}
}
