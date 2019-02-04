using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Catalog
{
    using Database;
    using Models;

    internal class CatalogDao : BaseDao
    {
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
    }
}
