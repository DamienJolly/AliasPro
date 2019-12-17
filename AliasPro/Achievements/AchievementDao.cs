using AliasPro.Achievements.Models;
using AliasPro.API.Achievements.Models;
using AliasPro.API.Configuration;
using AliasPro.API.Database;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Achievements
{
    internal class AchievementDao : BaseDao
    {
        public AchievementDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
			: base(logger, configurationController)
		{

		}

		public async Task<IDictionary<string, IAchievement>> ReadAchievements()
		{
			IDictionary<string, IAchievement> achievements = new Dictionary<string, IAchievement>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						IAchievement achievement = new Achievement(reader);
						achievement.Levels =
							await ReadAchievementLevels(achievement.Id);

						if (!achievements.ContainsKey(achievement.Name))
							achievements.Add(achievement.Name, achievement);
					}
				}, "SELECT * FROM `achievements`;");
			});
			return achievements;
		}

		public async Task<IList<IAchievementLevel>> ReadAchievementLevels(int id)
		{
			IList<IAchievementLevel> levels = new List<IAchievementLevel>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						levels.Add(new AchievementLevel(reader));
					}
				}, "SELECT * FROM `achievement_levels` WHERE `id` = @0;", id);
			});
			return levels;
		}

		public async Task AddPlayerAchievementAsync(int id, int progress, uint playerId)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "INSERT INTO `player_achievements` (`id`, `player_id`, `progress`) VALUES (@0, @1, @2);",
					id, playerId, progress);
			});
		}

		public async Task UpdatePlayerAchievementAsync(int id, int progress, uint playerId)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "UPDATE `player_achievements` set `progress` = @2 WHERE `id` = @0 AND `player_id` = @1;",
					id, playerId, progress);
			});
		}
	}
}
