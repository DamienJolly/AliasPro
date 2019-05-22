using AliasPro.Achievements.Models;
using AliasPro.API.Achievements.Models;
using AliasPro.API.Configuration;
using AliasPro.API.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Achievements
{
    internal class AchievementDao : BaseDao
    {
        public AchievementDao(IConfigurationController configurationController)
            : base(configurationController)
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
	}
}
