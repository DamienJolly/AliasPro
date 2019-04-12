using AliasPro.Catalog.Models;
using AliasPro.Items;
using System.Collections.Generic;

namespace AliasPro.Catalog
{
    internal class CatalogRepostiory
    {
        private readonly CatalogDao _catalogDao;
        private readonly ItemRepository _itemRepository;
        private IDictionary<int, ICatalogPage> _catalogPages;

        public CatalogRepostiory(CatalogDao catalogDao, ItemRepository itemRepository)
        {
            _catalogDao = catalogDao;
            _itemRepository = itemRepository;
            _catalogPages = new Dictionary<int, ICatalogPage>();

            InitializeCatalog();
        }

        public async void InitializeCatalog()
        {
            _catalogPages = await _catalogDao.GetCatalogPages();
            await _catalogDao.GetCatalogItems(this, _itemRepository);
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
