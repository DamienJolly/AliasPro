using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Room.Models.Item
{
    using AliasPro.Item.Models;

    public class ItemHandler
    {
        private readonly IRoom _room;
        private readonly IDictionary<uint, IItem> _items;

        internal ItemHandler(IRoom room)
        {
            _room = room;
            _items = new Dictionary<uint, IItem>();
        }

        internal void AddItem(IItem item)
        {
            if (!_items.ContainsKey(item.Id))
            {
                _items.Add(item.Id, item);
            }
        }

        internal void RemoveItem(uint itemId) =>
            _items.Remove(itemId);

        internal bool TryGetItem(uint itemId, out IItem item) =>
            _items.TryGetValue(itemId, out item);

        internal ICollection<IItem> Items =>
            _items.Values;

        internal ICollection<IItem> FloorItems =>
            _items.Values.Where(item => item.ItemData.Type == "s").ToList();

        internal ICollection<IItem> WallItems =>
            _items.Values.Where(item => item.ItemData.Type == "i").ToList();
    }
}
