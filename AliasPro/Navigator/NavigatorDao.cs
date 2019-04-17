using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Navigator.Models;
using AliasPro.Navigator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator
{
    internal class NavigatorDao : BaseDao
    {
        public NavigatorDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

        internal async Task<IDictionary<string, IDictionary<uint, INavigatorCategory>>> GetNavigatorCategoriesAsync()
        {
            IDictionary<string, IDictionary<uint, INavigatorCategory>> categories = new Dictionary<string, IDictionary<uint, INavigatorCategory>>();

            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        INavigatorCategory category = new NavigatorCategory(reader);

                        if (!categories.ContainsKey(category.Category))
                            categories.Add(category.Category, new Dictionary<uint, INavigatorCategory>());

                        if (!categories[category.Category].ContainsKey(category.Id))
                            categories[category.Category].Add(category.Id, category);
                    }
                }, "SELECT `id`, `min_rank`, `public_name`, `category_type`, `identifier`, `category` FROM `navigator_categories` ORDER BY `id`;");
            });

            return categories;
        }
    }
}
