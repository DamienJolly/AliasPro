using AliasPro.API.Database;
using AliasPro.Game.Badges.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Game.Badges
{
	internal class BadgeDao : BaseDao
	{
		public BadgeDao(ILogger<BaseDao> logger)
			: base(logger)
		{

		}

		public async Task<Dictionary<string, BadgeData>> ReadBadges()
		{
			Dictionary<string, BadgeData> badges = new Dictionary<string, BadgeData>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						BadgeData badge = new BadgeData(reader);
						badges.TryAdd(badge.Code, badge);
					}
				}, "SELECT * FROM `badges`;");
			});
			return badges;
		}
	}
}
