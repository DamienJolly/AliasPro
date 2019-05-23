using AliasPro.Achievements.Types;
using AliasPro.API.Players.Models;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.API.Achievements.Models
{
	public interface IAchievement
	{
		void Compose(ServerPacket message, IPlayer player);
		IAchievementLevel GetLevelForProgress(int progress);
		IAchievementLevel GetNextLevel(int currentLevel);

		int Id { get; set; }
		string Name { get; set; }
		AchievementCategory Category { get; set; }
		IList<IAchievementLevel> Levels { get; set; }
	}
}
