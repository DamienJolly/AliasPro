using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator
{
    using Database;
    using Models;

    internal class NavigatorDao : BaseDao
    {
        internal async Task<IList<INavigatorCategory>> GetNavigatorCategoriesAsync()
        {
            IList<INavigatorCategory> categories = new List<INavigatorCategory>();

            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(new NavigatorCategory(reader));
                    }
                }, "SELECT `id`, `min_rank`, `public_name`, `category_type`, `identifier`, `category` FROM `navigator_categories` ORDER BY `id`;");
            });

            return categories;
        }
    }
}
