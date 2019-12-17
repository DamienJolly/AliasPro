using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Navigator.Models;
using AliasPro.Navigator.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator
{
    internal class NavigatorDao : BaseDao
    {
        public NavigatorDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
            : base(logger, configurationController)
        {

        }

        internal async Task<IDictionary<string, INavigatorCategory>> GetNavigatorCategoriesAsync()
        {
            IDictionary<string, INavigatorCategory> categories = new Dictionary<string, INavigatorCategory>();

            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        INavigatorCategory category = new NavigatorCategory(reader);

                        if (!categories.ContainsKey(category.Identifier))
                            categories.Add(category.Identifier, category);
                    }
                }, "SELECT * FROM `navigator_categories` ORDER BY `sort_id`;");
            });

            return categories;
        }
    }
}
