using AliasPro.API.Achievements.Models;
using AliasPro.API.Players.Models;
using System.Collections.Generic;

namespace AliasPro.Players.Components
{
    public class AchievementComponent
	{
        private readonly IDictionary<int, IPlayerAchievement> _achievements;

        public AchievementComponent(
            IDictionary<int, IPlayerAchievement> achievements)
        {
			_achievements = achievements;
        }

		public bool HasAchieved(IAchievement achievement)
		{
			if (!GetAchievementProgress(achievement.Id, out IPlayerAchievement myAchievement))
				return false;

			IAchievementLevel level = achievement.GetLevelForProgress(myAchievement.Progress);

			if (level == null)
				return false;

			IAchievementLevel nextLevel = achievement.GetNextLevel(level.Level);

			return nextLevel == null && myAchievement.Progress >= level.Progress;
		}

		public void AddAchievement(IPlayerAchievement achievement) =>
			_achievements.Add(achievement.Id, achievement);

		public bool GetAchievementProgress(int id, out IPlayerAchievement achievement) => 
			_achievements.TryGetValue(id, out achievement);
	}
}
