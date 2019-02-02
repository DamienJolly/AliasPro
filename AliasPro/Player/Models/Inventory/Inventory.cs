using System.Collections.Generic;

namespace AliasPro.Player.Models.Inventory
{
    internal class Inventory : IInventory
    {
        public IDictionary<uint, IInventoryItem> Items { get; }

        internal Inventory(IDictionary<uint, IInventoryItem> items)
        {
            Items = items;
        }

        public bool TryGetItem(uint id, out IInventoryItem item) => Items.TryGetValue(id, out item);
    }

    public interface IInventory
    {
        IDictionary<uint, IInventoryItem> Items { get; }
        bool TryGetItem(uint id, out IInventoryItem item);
    }
}
