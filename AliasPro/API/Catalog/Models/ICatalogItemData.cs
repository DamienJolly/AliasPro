using AliasPro.Items.Models;

namespace AliasPro.API.Catalog.Models
{
    public interface ICatalogItemData
    {
        int Id { get; }
        int Amount { get; }
        IItemData ItemData { get; }
    }
}
