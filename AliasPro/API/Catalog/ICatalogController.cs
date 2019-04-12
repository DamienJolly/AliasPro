using AliasPro.API.Catalog.Models;
using System.Collections.Generic;

namespace AliasPro.API.Catalog
{
    public interface ICatalogController
    {
        ICollection<ICatalogPage> GetCatalogPages(int pageId, int rank);

        bool TryGetCatalogPage(int pageId, out ICatalogPage page);
        void ReloadCatalog();
    }
}
