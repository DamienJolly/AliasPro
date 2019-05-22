using AliasPro.API.Achievements;
using AliasPro.API.Achievements.Models;
using System.Collections.Generic;

namespace AliasPro.Achievements
{
    internal class AchievementController : IAchievementController
	{
		private readonly AchievementDao _achievementDao;

		private IDictionary<string, IAchievement> _achievements;

		public AchievementController(AchievementDao achievementDao)
        {
			_achievementDao = achievementDao;

			_achievements = new Dictionary<string, IAchievement>();

			InitializeAchievements();
		}

		public async void InitializeAchievements()
		{
			_achievements = await _achievementDao.ReadAchievements();
		}

		public ICollection<IAchievement> Achievements =>
			_achievements.Values;

		public bool TryGetAchievement(string name, out IAchievement achievement) =>
			_achievements.TryGetValue(name, out achievement);
	}
}
