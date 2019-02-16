using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator
{
    using Configuration;
    using Database;
    using Models;

    internal class NavigatorDao : BaseDao
    {
        public NavigatorDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

        internal async Task<IDictionary<string, ICollection<INavigatorCategory>>> GetNavigatorCategoriesAsync()
        {
            IDictionary<string, ICollection<INavigatorCategory>> categories = new Dictionary<string, ICollection<INavigatorCategory>>();

            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        INavigatorCategory category = new NavigatorCategory(reader);

                        if (!categories.ContainsKey(category.Category))
                            categories.Add(category.Category, new List<INavigatorCategory>());

                        categories[category.Category].Add(category);
                    }
                }, "SELECT `id`, `min_rank`, `public_name`, `category_type`, `identifier`, `category` FROM `navigator_categories` ORDER BY `id`;");
            });

            return categories;
        }
    }
}
