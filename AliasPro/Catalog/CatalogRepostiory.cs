using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Catalog
{
    using Models;

    internal class CatalogRepostiory
    {
        private readonly CatalogDao _catalogDao;
        private IDictionary<int, ICatalogPage> _catalogPages;

        public CatalogRepostiory(CatalogDao catalogDao)
        {
            _catalogDao = catalogDao;
        }

        public bool TryGetCatalogPage(int pageId, out ICatalogPage page) =>
            _catalogPages.TryGetValue(pageId, out page);

        public async Task<ICollection<ICatalogPage>> GetCatalogPagesAsync(int pageId, int rank)
        {
            if (_catalogPages == null) _catalogPages = await _catalogDao.GetCatalogPages();
            IList<ICatalogPage> pages = new List<ICatalogPage>();
            foreach (ICatalogPage page in _catalogPages.Values)
            {
                if (page.ParentId == pageId && page.Visible && page.Rank <= rank)
                {
                    pages.Add(page);
                }
            }
            return pages;
        }
    }
}
