using AliasPro.API.Catalog;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Players.Models;
using AliasPro.Groups.Types;
using AliasPro.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Catalog
{
    internal class CatalogController : ICatalogController
    {
        private readonly CatalogDao _catalogDao;
        private readonly IItemController _itemController;
        private IDictionary<int, ICatalogPage> _catalogPages;
        private IDictionary<int, ICatalogBot> _catalogBots;
        private IDictionary<int, ICatalogGiftPart> _giftParts;
        private IDictionary<int, int> _recyclerLevels;
        private IDictionary<int, IList<IItemData>> _recyclerPrizes;

        public CatalogController(
            CatalogDao catalogDao,
            IItemController itemController)
        {
            _catalogDao = catalogDao;
            _itemController = itemController;
            _catalogPages = new Dictionary<int, ICatalogPage>();
            _catalogBots = new Dictionary<int, ICatalogBot>();
            _giftParts = new Dictionary<int, ICatalogGiftPart>();
            _recyclerLevels = new Dictionary<int, int>();
            _recyclerPrizes = new Dictionary<int, IList<IItemData>>();

            InitializeCatalog();
        }

        public async void InitializeCatalog()
        {
            _catalogPages = await _catalogDao.GetCatalogPages();
            _catalogBots = await _catalogDao.GetCatalogBots();
            _giftParts = await _catalogDao.GetGiftParts();
            _recyclerLevels = await _catalogDao.GetRecyclerLevels();
            _recyclerPrizes = await _catalogDao.GetRecyclerPrizes();

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

        public IDictionary<int, IList<IItemData>> GetRecyclerPrizes =>
            _recyclerPrizes;

        public IDictionary<int, int> GetRecyclerLevels =>
            _recyclerLevels;

        public IItemData RecyclerPrize
        {
            get
            {
                IItemData itemData = null;
                int level = 1;

                foreach (var levels in _recyclerLevels)
                {
                    if (levels.Key > level && Randomness.RandomNumber(levels.Value + 1) == levels.Value)
                        level = levels.Key;
                }

                if (_recyclerPrizes.ContainsKey(level) && _recyclerPrizes[level].Count != 0)
                    itemData = _recyclerPrizes[level][Randomness.RandomNumber(_recyclerPrizes.Count) - 1];

                return itemData;
            }
        }
    }
}