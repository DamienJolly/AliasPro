using AliasPro.API.Badges.Models;
using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.Badges.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Badges
{
    internal class BadgeDao : BaseDao
    {
        public BadgeDao(IConfigurationController configurationController)
            : base(configurationController)
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

		public async Task AddPlayerBadge(uint playerId, string code)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "INSERT INTO `player_badges` (`player_id`, `code`) VALUES (@0, @1);",
					playerId, code);
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
