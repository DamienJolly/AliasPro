using AliasPro.API.Catalog.Models;
using AliasPro.Items;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Catalog
{
    internal class CatalogRepostiory
    {
        private readonly CatalogDao _catalogDao;
        private readonly ItemRepository _itemRepository;
        private IDictionary<int, ICatalogPage> _catalogPages;
		private IDictionary<int, ICatalogBot> _catalogBots;

		public CatalogRepostiory(CatalogDao catalogDao, ItemRepository itemRepository)
        {
            _catalogDao = catalogDao;
            _itemRepository = itemRepository;
            _catalogPages = new Dictionary<int, ICatalogPage>();
			_catalogBots = new Dictionary<int, ICatalogBot>();

			InitializeCatalog();
        }

        public async void InitializeCatalog()
        {
            _catalogPages = await _catalogDao.GetCatalogPages();
			_catalogBots = await _catalogDao.GetCatalogBots();
			await _catalogDao.GetCatalogItems(this, _itemRepository);
		}

		public bool TryGetCatalogBot(int itemId, out ICatalogBot bot) =>
			_catalogBots.TryGetValue(itemId, out bot);

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

        public async Task AddLimitedAsync(uint itemId, uint playerId, int number) =>
            await _catalogDao.AddLimitedAsync(itemId, playerId, number);
    }
}
