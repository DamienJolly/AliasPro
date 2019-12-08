using AliasPro.API.Catalog.Models;
using AliasPro.API.Items;
using AliasPro.API.Players.Models;
using AliasPro.Groups.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Catalog
{
    internal class CatalogRepostiory
    {
        private readonly CatalogDao _catalogDao;
        private readonly IItemController _itemController;
        private IDictionary<int, ICatalogPage> _catalogPages;
		private IDictionary<int, ICatalogBot> _catalogBots;
		private IDictionary<int, ICatalogGiftPart> _giftParts;

		public CatalogRepostiory(CatalogDao catalogDao, IItemController itemController)
        {
            _catalogDao = catalogDao;
			_itemController = itemController;
            _catalogPages = new Dictionary<int, ICatalogPage>();
			_catalogBots = new Dictionary<int, ICatalogBot>();
			_giftParts = new Dictionary<int, ICatalogGiftPart>();

			InitializeCatalog();
        }

        public async void InitializeCatalog()
        {
            _catalogPages = await _catalogDao.GetCatalogPages();
			_catalogBots = await _catalogDao.GetCatalogBots();
			_giftParts = await _catalogDao.GetGiftParts();
			await _catalogDao.GetCatalogItems(this, _itemController);
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

		public async Task<int> AddNewBotAsync(IPlayerBot playerBot, int playerId) =>
			await _catalogDao.AddNewBotAsync(playerBot, playerId);

		public async Task<int> AddNewPetAsync(IPlayerPet playerPet, int playerId) =>
			await _catalogDao.AddNewPetAsync(playerPet, playerId);

		public bool TryGetGift(int spriteId, out ICatalogGiftPart item) =>
			_giftParts.TryGetValue(spriteId, out item);

		public ICollection<ICatalogGiftPart> GetGifts =>
			_giftParts.Values.Where(part => part.Type == GiftPartType.GIFT).ToList();

		public ICollection<ICatalogGiftPart> GetWrappers =>
			_giftParts.Values.Where(part => part.Type == GiftPartType.WRAPPER).ToList();
	}
}
