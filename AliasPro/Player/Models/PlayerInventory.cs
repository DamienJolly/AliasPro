using System.Collections.Generic;

namespace AliasPro.Player.Models
{
    using Item.Models;

    internal class PlayerInventory : IPlayerInventory
    {
        public IDictionary<uint, IItem> Items { get; }

        internal PlayerInventory(IDictionary<uint, IItem> items)
        {
            Items = items;
        }

        public bool TryGetItem(uint id, out IItem item) => Items.TryGetValue(id, out item);
    }

    public interface IPlayerInventory
    {
        IDictionary<uint, IItem> Items { get; }
        bool TryGetItem(uint id, out IItem item);
    }
}
