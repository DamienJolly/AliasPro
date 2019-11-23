using AliasPro.API.Items.Models;
using AliasPro.API.Players.Models;
using System.Collections.Generic;

namespace AliasPro.Players.Components
{
    public class InventoryComponent
    {
        private readonly IDictionary<uint, IItem> _items;
		private readonly IDictionary<int, IPlayerBot> _bots;

		internal InventoryComponent(
            IDictionary<uint, IItem> items,
			IDictionary<int, IPlayerBot> bots)
        {
            _items = items;
			_bots = bots;
		}

        public ICollection<IItem> Items =>
            _items.Values;

		public ICollection<IPlayerBot> Bots =>
			_bots.Values;

		public bool TryAddItem(IItem item) =>
            _items.TryAdd(item.Id, item);

		public bool TryAddBot(IPlayerBot bot) =>
			_bots.TryAdd(bot.Id, bot);

		public void RemoveItem(uint itemId) =>
            _items.Remove(itemId);

		public void RemoveBot(int botId) =>
			_bots.Remove(botId);

		public bool TryGetItem(uint itemId, out IItem item) =>
            _items.TryGetValue(itemId, out item);

		public bool TryGetBot(int botId, out IPlayerBot bot) =>
			_bots.TryGetValue(botId, out bot);
	}
}
