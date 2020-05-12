using AliasPro.API.Database;
using AliasPro.Game.Achievements.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Game.Achievements
{
	public class AchievementDao : BaseDao
	{
		public AchievementDao(ILogger<BaseDao> logger)
			: base(logger)
		{

		}

		public async Task<Dictionary<string, AchievementData>> ReadAchievements()
		{
			Dictionary<string, AchievementData> achievements = new Dictionary<string, AchievementData>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						AchievementData achievement = new AchievementData(
							reader.ReadData<int>("id"),
							reader.ReadData<string>("name"),
							reader.ReadData<string>("category"));

						achievement.Levels = await ReadAchievementLevels(achievement.Id);

						if (!achievements.ContainsKey(achievement.Name))
							achievements.Add(achievement.Name, achievement);
					}
				}, "SELECT * FROM `achievements`;");
			});
			return achievements;
		}

		private async Task<Dictionary<int, AchievementLevel>> ReadAchievementLevels(int id)
		{
			Dictionary<int, AchievementLevel> levels = new Dictionary<int, AchievementLevel>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						AchievementLevel level = new AchievementLevel(
							reader.ReadData<int>("level"),
							reader.ReadData<int>("reward_amount"),
							reader.ReadData<int>("reward_type"),
							reader.ReadData<int>("reward_points"),
							reader.ReadData<int>("progress"));

						if (!levels.ContainsKey(level.Level))
							levels.Add(level.Level, level);
					}
				}, "SELECT * FROM `achievement_levels` WHERE `id` = @0;", id);
			});
			return levels;
		}
	}
}
