using AliasPro.Catalog.Models;
using AliasPro.Configuration;
using AliasPro.Database;
using AliasPro.Items;
using AliasPro.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Catalog
{
    internal class CatalogDao : BaseDao
    {
        public CatalogDao(IConfigurationController configurationController) 
            : base(configurationController)
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

        internal async Task GetCatalogItems(CatalogRepostiory catalogRepostiory, ItemRepository itemRepository)
        {
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        ICatalogItem item = new CatalogItem(reader, itemRepository);

                        if (item.IsLimited)
                        {
                            item.LimitedNumbers = 
                                await ReadLimited(item.Id, item.LimitedStack);
                            item.LimitedNumbers.Shuffle();
                        }
                        
                        if (catalogRepostiory.TryGetCatalogPage(item.PageId, out ICatalogPage page))
                        {
                            page.Items.Add(item.Id, item);
                        }
                    }
                }, "SELECT * FROM `catalog_items`;");
            });
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
    }
}
