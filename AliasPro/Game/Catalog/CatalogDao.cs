using AliasPro.API.Database;
using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Players.Models;
using AliasPro.Game.Catalog.Models;
using AliasPro.Players.Types;
using AliasPro.Utilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog
{
	public class CatalogDao : BaseDao
	{
		private readonly ILogger<BaseDao> logger;

		public CatalogDao(ILogger<BaseDao> logger)
			: base(logger)
		{
			this.logger = logger;
		}

		public async Task<Dictionary<int, CatalogItem>> ReadCatalogItems(Dictionary<int, CatalogPage> catalogPages)
		{
			Dictionary<int, CatalogItem> catalogItems = new Dictionary<int, CatalogItem>();

			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						CatalogItem catalogItem = new CatalogItem(
							reader.ReadData<int>("id"),
							reader.ReadData<int>("page_Id"),
							reader.ReadData<string>("catalog_name"),
							reader.ReadData<int>("cost_credits"),
							reader.ReadData<int>("cost_points"),
							reader.ReadData<int>("points_type"),
							reader.ReadData<int>("club_level"),
							reader.ReadData<bool>("can_gift"),
							reader.ReadData<bool>("have_offer"),
							reader.ReadData<int>("offer_id"),
							reader.ReadData<int>("bot_id"),
							reader.ReadData<string>("badge"),
							reader.ReadData<int>("limited_stack")
						);

						string itemParts = reader.ReadData<string>("item_ids");
						foreach (string itemPart in itemParts.Split(':'))
						{
							try
							{
								string[] data = itemPart.Split('*');

								int itemId = int.Parse(data[0]);
								int amount = 1;
								if (data.Length >= 2)
									amount = int.Parse(data[1]);
								string extraData = "";

								if (!Program.GetService<IItemController>().TryGetItemDataById((uint)itemId, out IItemData itemData))
								{
									logger.LogError("Failed to load ItemData for [" + itemId + "] this item will be skipped.");
									continue;
								}

								if (itemData.Type == "r" && catalogItem.BotId != 0)
								{
									CatalogBotData dotData = await ReadCatalogBotData(catalogItem.BotId);
									if (dotData == null)
									{
										logger.LogError("Failed to load BotData for [" + catalogItem.BotId + "] this item will be skipped.");
										continue;
									}
									extraData = dotData.Figure;
								}

								if (catalogItem.Name.Contains("wallpaper_single") || 
									catalogItem.Name.Contains("floor_single") || 
									catalogItem.Name.Contains("landscape_single"))
								{
									extraData = catalogItem.Name.Split('_')[2];
								}

								CatalogItemData catalogItemData = new CatalogItemData(
									itemId,
									amount,
									itemData,
									extraData
								);

								catalogItem.Items.Add(catalogItemData);
							}
							catch
							{
								logger.LogError("Failed to parse item_ids for CatalogItem [" + catalogItem.Id + "]");
							}
						}

						if (catalogItem.IsLimited)
						{
							catalogItem.LimitedNumbers =
								await ReadLimited(catalogItem.Id, catalogItem.LimitedStack);
							catalogItem.LimitedNumbers.Shuffle();
						}

						if (!catalogItems.ContainsKey(catalogItem.Id))
							catalogItems.Add(catalogItem.Id, catalogItem);

						if (catalogPages.ContainsKey(catalogItem.PageId))
						{
							catalogPages[catalogItem.PageId].Items.TryAdd(catalogItem.Id, catalogItem);

							if (catalogItem.OfferId != -1)
							{
								catalogPages[catalogItem.PageId].OfferIds.Add(catalogItem.OfferId);
							}
						}
					}
				}, "SELECT * FROM `catalog_items` ORDER BY `id` ASC;");
			});

			return catalogItems;
		}

		public async Task<Dictionary<int, CatalogPage>> ReadCatalogPages()
		{
			Dictionary<int, CatalogPage> pages = new Dictionary<int, CatalogPage>();

			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						CatalogPage page = new CatalogPage(
							reader.ReadData<int>("id"),
							reader.ReadData<int>("parent_id"),
							reader.ReadData<string>("name"),
							reader.ReadData<string>("caption"),
							reader.ReadData<int>("icon"),
							reader.ReadData<int>("rank"),
							reader.ReadData<string>("header_image"),
							reader.ReadData<string>("teaser_image"),
							reader.ReadData<string>("special_image"),
							reader.ReadData<string>("text_one"),
							reader.ReadData<string>("text_two"),
							reader.ReadData<string>("text_details"),
							reader.ReadData<string>("text_teaser"),
							reader.ReadData<string>("layout"),
							reader.ReadData<bool>("enabled"),
							reader.ReadData<bool>("visible")
						);

						if (!pages.ContainsKey(page.Id))
							pages.Add(page.Id, page);
					}
				}, "SELECT * FROM `catalog_pages` ORDER BY `order_num` ASC, `caption` ASC;");
			});

			return pages;
		}

		public async Task<Dictionary<int, CatalogFeaturedPage>> ReadCatalogFeaturedPages()
		{
			Dictionary<int, CatalogFeaturedPage> featured = new Dictionary<int, CatalogFeaturedPage>();

			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						CatalogFeaturedPage feature = new CatalogFeaturedPage(
							reader.ReadData<int>("slot_id"),
							reader.ReadData<string>("caption"),
							reader.ReadData<string>("image"),
							reader.ReadData<string>("type"),
							reader.ReadData<string>("page_name"),
							reader.ReadData<int>("page_id"),
							reader.ReadData<string>("product_name"),
							reader.ReadData<int>("expire_timestamp")
						);

						if (!featured.ContainsKey(feature.SlotId))
							featured.Add(feature.SlotId, feature);
					}
				}, "SELECT * FROM `catalog_featured_pages`;");
			});
			return featured;
		}

		public async Task<CatalogBotData> ReadCatalogBotData(int botId)
		{
			CatalogBotData botData = null;

			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					if (await reader.ReadAsync())
					{
						CatalogBotData bot = new CatalogBotData(
							reader.ReadData<int>("item_id"),
							reader.ReadData<string>("name"),
							reader.ReadData<string>("motto"),
							reader.ReadData<string>("figure"),
							reader.ReadData<string>("gender")
						);

						botData = bot;
					}
				}, "SELECT * FROM `catalog_bots` WHERE `item_id` = @0 LIMIT 1;", botId);
			});

			return botData;
		}

		public async Task<Dictionary<int, CatalogBotData>> ReadCatalogBotsData()
		{
			Dictionary<int, CatalogBotData> bots = new Dictionary<int, CatalogBotData>();

			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						CatalogBotData bot = new CatalogBotData(
							reader.ReadData<int>("item_id"),
							reader.ReadData<string>("name"),
							reader.ReadData<string>("motto"),
							reader.ReadData<string>("figure"),
							reader.ReadData<string>("gender")
						);

						if (!bots.ContainsKey(bot.ItemId))
							bots.Add(bot.ItemId, bot);
					}
				}, "SELECT * FROM `catalog_bots`;");
			});

			return bots;
		}

		public async Task<Dictionary<int, CatalogGiftPartData>> ReadGiftPartsData()
		{
			Dictionary<int, CatalogGiftPartData> giftParts = new Dictionary<int, CatalogGiftPartData>();

			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						CatalogGiftPartData giftPart = new CatalogGiftPartData(
							reader.ReadData<int>("item_id"),
							reader.ReadData<int>("sprite_id"),
							reader.ReadData<string>("type")
						);

						if (!giftParts.ContainsKey(giftPart.SpriteId))
							giftParts.Add(giftPart.SpriteId, giftPart);
					}
				}, "SELECT * FROM `catalog_gift_parts`;");
			});

			return giftParts;
		}

		public async Task<Dictionary<int, int>> GetRecyclerLevels()
		{
			Dictionary<int, int> recyclerLevels = new Dictionary<int, int>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						int level = reader.ReadData<int>("level");
						int rarity = reader.ReadData<int>("rarity");

						if (!recyclerLevels.ContainsKey(level))
							recyclerLevels.Add(level, rarity);
					}
				}, "SELECT * FROM `catalog_recycler_levels` ORDER BY `level` DESC;");
			});
			return recyclerLevels;
		}

		public async Task<Dictionary<int, List<IItemData>>> GetRecyclerPrizes()
		{
			Dictionary<int, List<IItemData>> recyclerPrizes = new Dictionary<int, List<IItemData>>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						int level = reader.ReadData<int>("level");
						int itemId = reader.ReadData<int>("item_id");

						if (!Program.GetService<IItemController>().TryGetItemDataById((uint)itemId, out IItemData item))
							continue;

						if (!recyclerPrizes.ContainsKey(level))
							recyclerPrizes.Add(level, new List<IItemData>());

						recyclerPrizes[level].Add(item);
					}
				}, "SELECT * FROM `catalog_recycler_prizes` ORDER BY `level` DESC;");
			});
			return recyclerPrizes;
		}

		public async Task<List<int>> ReadLimited(int itemId, int size)
		{
			List<int> availableNumbers = new List<int>();
			List<int> takenNumbers = new List<int>();

			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						takenNumbers.Add(reader.ReadData<int>("number"));
					}
				}, "SELECT `number` FROM `catalog_items_limited` WHERE `item_id` = @0;", itemId);
			});

			for (int i = 1; i <= size; i++)
			{
				if (!takenNumbers.Contains(i))
				{
					availableNumbers.Add(i);
				}
			}
			return availableNumbers;
		}

		public async Task AddLimitedAsync(uint itemId, uint playerId, int number)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "INSERT INTO `catalog_limited_items` (`item_id`, `player_id`, `number`, `timestamp`) VALUES (@0, @1, @2, @3);",
					itemId, playerId, number, (int)UnixTimestamp.Now);
			});
		}

		public async Task<int> AddNewBotAsync(IPlayerBot playerBot, int playerId)
		{
			int botId = -1;
			await CreateTransaction(async transaction =>
			{
				botId = await Insert(transaction, "INSERT INTO `bots` (`player_id`, `name`, `motto`, `figure`, `gender`) VALUES (@0, @1, @2, @3, @4);",
					playerId, playerBot.Name, playerBot.Motto, playerBot.Figure, playerBot.Gender == PlayerGender.MALE ? "m" : "f");
			});
			return botId;
		}

		public async Task<int> AddNewPetAsync(IPlayerPet playerPet, int playerId)
		{
			int botId = -1;
			await CreateTransaction(async transaction =>
			{
				botId = await Insert(transaction, "INSERT INTO `player_pets` (`player_id`, `name`, `race`, `type`, `colour`, `created`) VALUES (@0, @1, @2, @3, @4, @5);",
					playerId, playerPet.Name, playerPet.Race, playerPet.Type, playerPet.Colour, playerPet.Created);
			});
			return botId;
		}
	}
}