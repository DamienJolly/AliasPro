using AliasPro.Achievements.Types;
using System.Collections.Generic;

namespace AliasPro.API.Achievements.Models
{
	public interface IAchievement
	{
		IAchievementLevel GetLevelForProgress(int progress);
		IAchievementLevel GetNextLevel(int currentLevel);

		int Id { get; set; }
		string Name { get; set; }
		AchievementCategory Category { get; set; }
		IList<IAchievementLevel> Levels { get; set; }
	}
}
