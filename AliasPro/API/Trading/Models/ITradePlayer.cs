using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using System.Collections.Generic;

namespace AliasPro.API.Trading.Models
{
	public interface ITradePlayer
	{
		BaseEntity Entity { get; set; }
		IDictionary<uint, IItem> OfferedItems { get; set; }

		bool TryAddItem(IItem item);
		void RemoveItem(uint itemId);
	}
}
