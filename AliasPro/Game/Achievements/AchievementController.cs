using AliasPro.Game.Achievements.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AliasPro.Game.Achievements
{
	public class AchievementController
	{
		private readonly ILogger<AchievementController> logger;
		private readonly AchievementDao achievementDao;

		private Dictionary<string, AchievementData> achievements;

		public AchievementController(
			ILogger<AchievementController> logger,
			AchievementDao achievementDao)
		{
			this.logger = logger;
			this.achievementDao = achievementDao;

			achievements = new Dictionary<string, AchievementData>();

			InitializeAchievements();

			this.logger.LogInformation("Loaded " + achievements.Count + " achievements.");
		}

		public async void InitializeAchievements()
		{
			achievements = await achievementDao.ReadAchievements();
		}

		public bool TryGetAchievement(string code, out AchievementData achievement) =>
			achievements.TryGetValue(code, out achievement);

		public ICollection<AchievementData> Achievements =>
			achievements.Values;
	}
}
