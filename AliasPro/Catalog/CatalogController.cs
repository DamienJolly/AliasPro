using System.Collections.Generic;

namespace AliasPro.Catalog
{
    using Models;

    internal class CatalogController : ICatalogController
    {
        private readonly CatalogRepostiory _catalogRepostiory;

        public CatalogController(CatalogRepostiory catalogRepostiory)
        {
            _catalogRepostiory = catalogRepostiory;
        }

        public bool TryGetCatalogPage(int pageId, out ICatalogPage page) =>
            _catalogRepostiory.TryGetCatalogPage(pageId, out page);

        public ICollection<ICatalogPage> GetCatalogPages(int pageId, int rank) =>
            _catalogRepostiory.GetCatalogPages(pageId, rank);

        public void ReloadCatalog() =>
            _catalogRepostiory.InitializeCatalog();
    }

    public interface ICatalogController
    {
        ICollection<ICatalogPage> GetCatalogPages(int pageId, int rank);
        bool TryGetCatalogPage(int pageId, out ICatalogPage page);
        void ReloadCatalog();
    }
}