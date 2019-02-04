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

            InitializeCatalog();
        }

        private async void InitializeCatalog()
        {
            _catalogPages = await _catalogDao.GetCatalogPages();
        }

        public bool TryGetCatalogPage(int pageId, out ICatalogPage page) =>
            _catalogPages.TryGetValue(pageId, out page);

        public ICollection<ICatalogPage> GetCatalogPages(int pageId, int rank)
        {
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
