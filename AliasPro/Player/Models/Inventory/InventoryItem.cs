using System.Data.Common;

namespace AliasPro.Player.Models.Inventory
{
    using Item.Models;
    using Database;

    internal class InventoryItem : IInventoryItem
    {
        internal InventoryItem(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            ItemId = reader.ReadData<uint>("item_id");
        }

        public uint Id { get; }
        public uint ItemId { get; }
        public IItemData ItemData { get; set; }
    }

    public interface IInventoryItem
    {
        uint Id { get; }
        uint ItemId { get; }
        IItemData ItemData { get; set; }
    }
}
