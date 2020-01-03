using AliasPro.API.Catalog.Models;
using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Players.Models;
using AliasPro.Catalog.Models;
using AliasPro.Items;
using AliasPro.Players.Types;
using AliasPro.Utilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Catalog
{
    internal class CatalogDao : BaseDao
    {
        public CatalogDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
			: base(logger, configurationController)
		{

		}

		internal async Task<IDictionary<int, ICatalogPage>> GetCatalogPages()
        {
            IDictionary<int, ICatalogPage> pages = new Dictionary<int, ICatalogPage>();

            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        ICatalogPage page = new CatalogPage(reader);
                        pages.Add(page.Id, page);
                    }
                }, "SELECT * FROM `catalog_pages` ORDER BY `order_num` ASC, `caption` ASC;");
            });

            return pages;
        }

        internal async Task GetCatalogItems(CatalogRepostiory catalogRepostiory, IItemController itemRepository)
        {
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        ICatalogItem catalogItem = new CatalogItem(reader);

						string itemIds = reader.ReadData<string>("item_ids");
						foreach (string items in itemIds.Split(':'))
						{
							string[] data = items.Split('*');
							int amount = 1;

							if (itemRepository.TryGetItemDataById(uint.Parse(data[0]), out IItemData itemData))
							{
								if (data.Length >= 2)
									amount = int.Parse(data[1]);

								ICatalogItemData catalogItemData = new CatalogItemData((int)itemData.Id, amount, itemData);

								if (itemData.Type == "r" && catalogRepostiory.TryGetCatalogBot((int)itemData.Id, out ICatalogBot botData))
									catalogItemData.BotData = botData;

								catalogItem.Items.Add(catalogItemData);
							}
						}

						if (catalogItem.IsLimited)
                        {
							catalogItem.LimitedNumbers = 
                                await ReadLimited(catalogItem.Id, catalogItem.LimitedStack);
							catalogItem.LimitedNumbers.Shuffle();
                        }

						if (catalogRepostiory.TryGetCatalogPage(catalogItem.PageId, out ICatalogPage page))
							page.Items.Add(catalogItem.Id, catalogItem);
					}
                }, "SELECT * FROM `catalog_items` ORDER BY `id` ASC;");
            });
        }

		internal async Task<IDictionary<int, ICatalogBot>> GetCatalogBots()
		{
			IDictionary<int, ICatalogBot> bots = new Dictionary<int, ICatalogBot>();

			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						ICatalogBot bot = new CatalogBot(reader);

						if (!bots.ContainsKey(bot.ItemId))
							bots.Add(bot.ItemId, bot);
					}
				}, "SELECT * FROM `catalog_bots`;");
			});

			return bots;
		}

		public async Task<IDictionary<int, ICatalogGiftPart>> GetGiftParts()
		{
			IDictionary<int, ICatalogGiftPart> giftParts = new Dictionary<int, ICatalogGiftPart>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						ICatalogGiftPart giftPart = new CatalogGiftPart(reader);

						if (!giftParts.ContainsKey(giftPart.SpriteId))
							giftParts.Add(giftPart.SpriteId, giftPart);
					}
				}, "SELECT * FROM `catalog_gift_parts`;");
			});
			return giftParts;
		}

		internal async Task<List<int>> ReadLimited(int itemId, int size)
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

        internal async Task AddLimitedAsync(uint itemId, uint playerId, int number)
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
