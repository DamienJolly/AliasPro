using AliasPro.API.Items.Models;
using AliasPro.Game.Catalog.Models;
using AliasPro.Game.Catalog.Purchase;
using AliasPro.Game.Catalog.Types;
using AliasPro.Utilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Game.Catalog
{
	public class CatalogController
	{
		private readonly ILogger<CatalogController> logger;
		private readonly CatalogDao catalogDao;

		private Dictionary<int, CatalogItem> catalogItems;
		private Dictionary<int, CatalogPage> catalogPages;
		private Dictionary<int, CatalogFeaturedPage> featuredPages;
		private Dictionary<int, CatalogBotData> catalogBotsData;
		private Dictionary<int, CatalogGiftPartData> giftPartsData;

		private Dictionary<int, int> recyclerLevels;
		private Dictionary<int, List<IItemData>> recyclerPrizes;

		public CatalogPurchaseHandler PurchaseHandler;

		public CatalogController(
			ILogger<CatalogController> logger,
			CatalogDao catalogDao)
		{
			this.logger = logger;
			this.catalogDao = catalogDao;

			PurchaseHandler = new CatalogPurchaseHandler();

			InitializeCatalog();

			this.logger.LogInformation("Loaded " + catalogPages.Count + " catalog pages & " + catalogItems.Count + " catalog items.");
		}

		public async void InitializeCatalog()
		{
			featuredPages = await catalogDao.ReadCatalogFeaturedPages();
			catalogBotsData = await catalogDao.ReadCatalogBotsData();
			giftPartsData = await catalogDao.ReadGiftPartsData();

			catalogPages = await catalogDao.ReadCatalogPages();
			catalogItems = await catalogDao.ReadCatalogItems(catalogPages);

			recyclerLevels = await catalogDao.GetRecyclerLevels();
			recyclerPrizes = await catalogDao.GetRecyclerPrizes();
		}

		public bool TryGetRecyclerPrize(out IItemData prize)
		{
			prize = null;
			int level = 1;

			foreach (KeyValuePair<int, int> levels in recyclerLevels)
			{
				if (levels.Key > level && Randomness.RandomNumber(levels.Value + 1) == levels.Value)
				{
					level = levels.Key;
				}
			}

			if (recyclerPrizes.ContainsKey(level) && recyclerPrizes[level].Count != 0)
			{
				prize = recyclerPrizes[level][Randomness.RandomNumber(recyclerPrizes.Count) - 1];
			}

			return prize != null;
		}

		public bool TryGetCatalogPage(int pageId, out CatalogPage page) =>
			catalogPages.TryGetValue(pageId, out page);

		public ICollection<CatalogPage> GetCatalogPages(int pageId, int rank) => 
			catalogPages.Values.Where(page => page.ParentId == pageId && page.Visible && page.Rank <= rank).ToList();

		public ICollection<CatalogFeaturedPage> FeaturedPages =>
			featuredPages.Values;

		public bool TryGetBotData(int botId, out CatalogBotData botData) =>
			catalogBotsData.TryGetValue(botId, out botData);

		public bool TryGetCatalogItem(int itemId, out CatalogItem item) =>
			catalogItems.TryGetValue(itemId, out item);

		public bool TryGetCatalogItemByOfferId(int offerId, out CatalogItem item)
		{
			item = catalogItems.Values.Where(data => data.OfferId == offerId).FirstOrDefault();
			return item != null;
		}

		public bool TryGetCatalogGiftData(int spriteId, out CatalogGiftPartData item) =>
			giftPartsData.TryGetValue(spriteId, out item);

		public ICollection<CatalogGiftPartData> GetGifts =>
			giftPartsData.Values.Where(part => part.Type == CatalogGiftPartType.GIFT).ToList();

		public ICollection<CatalogGiftPartData> GetWrappers =>
			giftPartsData.Values.Where(part => part.Type == CatalogGiftPartType.WRAPPER).ToList();

		public Dictionary<int, List<IItemData>> GetRecyclerPrizes =>
			recyclerPrizes;

		public Dictionary<int, int> GetRecyclerLevels =>
			recyclerLevels;
	}
}
