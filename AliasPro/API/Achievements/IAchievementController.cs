using AliasPro.API.Achievements.Models;
using System.Collections.Generic;

namespace AliasPro.API.Achievements
{
    public interface IAchievementController
	{
		void InitializeAchievements();
		ICollection<IAchievement> Achievements { get; }
		bool TryGetAchievement(string name, out IAchievement achievement);
	}
}
