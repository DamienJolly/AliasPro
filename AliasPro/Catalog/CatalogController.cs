using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<ICollection<ICatalogPage>> GetCatalogPagesAsync(int pageId, int rank) =>
            await _catalogRepostiory.GetCatalogPagesAsync(pageId, rank);
    }

    public interface ICatalogController
    {
        Task<ICollection<ICatalogPage>> GetCatalogPagesAsync(int pageId, int rank);
        bool TryGetCatalogPage(int pageId, out ICatalogPage page);
    }
}