using AliasPro.API.Badges.Models;
using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using AliasPro.Badges.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Badges
{
    internal class BadgeDao : BaseDao
    {
        public BadgeDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
			: base(logger, configurationController)
		{

		}

		public async Task<IDictionary<string, IBadge>> ReadBadges()
		{
			IDictionary<string, IBadge> badges = new Dictionary<string, IBadge>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						IBadge badge = new Badge(reader);

						if (!badges.ContainsKey(badge.Code))
							badges.Add(badge.Code, badge);
					}
				}, "SELECT * FROM `badges`;");
			});
			return badges;
		}

		public async Task AddPlayerBadge(uint playerId, IPlayerBadge badge)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "INSERT INTO `player_badges` (`player_id`, `badge_id`, `code`) VALUES (@0, @1, @2);",
					playerId, badge.BadgeId, badge.Code);
			});
		}

		public async Task UpdatePlayerBadge(uint playerId, string oldCode, string newCode)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "UPDATE `player_badges` set `code` = @2 WHERE `player_id` = @0 AND `code` = @1;",
					playerId, oldCode, newCode);
			});
		}

		public async Task RemovePlayerBadge(uint playerId, string code)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "DELETE FROM `player_badges` WHERE `player_id` = @0 AND `code` = @1;",
					playerId, code);
			});
		}
	}
}
