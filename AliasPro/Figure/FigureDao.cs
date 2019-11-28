using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Figure.Models;
using AliasPro.Figure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Figure
{
    internal class FigureDao : BaseDao
    {
        public FigureDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

		internal async Task<IDictionary<int, IWardrobeItem>> GetPlayerWardrobeAsync(uint id)
		{
			IDictionary<int, IWardrobeItem> items = new Dictionary<int, IWardrobeItem>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						IWardrobeItem item = new WardrobeItem(reader);
						if (!items.TryAdd(item.SlotId, item))
						{
							// failed
						}
					}
				}, "SELECT * FROM `player_wardrobe` WHERE `player_id` = @0;", id);
			});
			return items;
		}
	}
}