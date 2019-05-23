using AliasPro.API.Achievements.Models;
using AliasPro.API.Players.Models;
using System.Collections.Generic;

namespace AliasPro.API.Achievements
{
    public interface IAchievementController
	{
		void InitializeAchievements();
		ICollection<IAchievement> Achievements { get; }
		void ProgressAchievement(IPlayer player, string name, int amount = 1);
		bool TryGetAchievement(string name, out IAchievement achievement);
	}
}
