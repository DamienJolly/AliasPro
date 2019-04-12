using AliasPro.Items.Models;
using System.Collections.Generic;

namespace AliasPro.Players.Components
{
    public class InventoryComponent
    {
        private readonly IDictionary<uint, IItem> _items;

        internal InventoryComponent(
            IDictionary<uint, IItem> items)
        {
            _items = items;
        }

        public ICollection<IItem> Items =>
            _items.Values;

        public bool TryAddItem(IItem item) =>
            _items.TryAdd(item.Id, item);

        public void RemoveItem(uint itemId) =>
            _items.Remove(itemId);

        public bool TryGetItem(uint itemId, out IItem item) =>
            _items.TryGetValue(itemId, out item);
    }
}
