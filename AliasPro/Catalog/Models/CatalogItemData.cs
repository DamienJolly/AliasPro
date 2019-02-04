namespace AliasPro.Catalog.Models
{
    using Item.Models;

    internal class CatalogItemData : ICatalogItemData
    {
        internal CatalogItemData(int id, int amount, IItemData itemData)
        {
            Id = id;
            Amount = amount;
            ItemData = itemData;
        }

        public int Id { get; }
        public int Amount { get; }
        public IItemData ItemData { get; }
    }

    public interface ICatalogItemData
    {
        int Id { get; }
        int Amount { get; }
        IItemData ItemData { get; }
    }
}
