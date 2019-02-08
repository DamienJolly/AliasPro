using System.Collections.Generic;

namespace AliasPro.Room.Gamemap
{
    using Models;
    using Item.Models;

    public class RoomTile
    {
        private readonly IRoom _room;
        private readonly IDictionary<uint, IItem> _items;

        internal RoomTile(IRoom room)
        {
            _room = room;
            _items = new Dictionary<uint, IItem>();
        }

        public bool CanWalkOn()
        {
            IItem topItem = GetTopItem();
            if (topItem != null)
                return topItem.ItemData.CanWalk;
            return true;
        }

        public IItem GetTopItem()
        {
            IItem topItem = null;
            foreach (IItem item in _items.Values)
            {
                if (topItem == null)
                {
                    topItem = item;
                    continue;
                }
                if (item.Position.Z + item.ItemData.Height >
                    topItem.Position.Z + topItem.ItemData.Height)
                    topItem = item;
            }
            return topItem;
        }

        public void AddItem(IItem item)
        {
            if (!_items.ContainsKey(item.Id))
            {
                _items.Add(item.Id, item);
            }
        }

        public void RemoveItem(uint itemId) =>
            _items.Remove(itemId);
    }
}
