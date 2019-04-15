using AliasPro.API.Catalog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Catalog
{
    public interface ICatalogController
    {
        ICollection<ICatalogPage> GetCatalogPages(int pageId, int rank);

        bool TryGetCatalogPage(int pageId, out ICatalogPage page);
        void ReloadCatalog();
        Task AddLimitedAsync(uint itemId, uint playerId, int number);
    }
}
