using AliasPro.Catalog.Types;

namespace AliasPro.API.Catalog.Models
{
    public interface ICatalogFeaturedPage
    {
        int SlotId { get; }
        string Caption { get; }
        string Image { get; }
        FeaturedPageType Type { get; }
        string PageName { get; }
        int PageId { get; }
        string ProductName { get; }
        int ExpireTimestamp { get; }
    }
}
