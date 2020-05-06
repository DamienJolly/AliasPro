using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Achievements.Types;
using AliasPro.Game.Achievements.Utilities;
using System.Collections.Generic;

namespace AliasPro.Game.Achievements.Models
{
	public class AchievementData
	{
		public int Id { get; }
		public string Name { get; }
		public AchievementCategory Category { get; }
		public Dictionary<int, AchievementLevel> Levels { get; set; }

		public AchievementData(int id, string name, string category)
		{
			Id = id;
			Name = name;
			Category = AchievementCategoryUtility.GetCategory(category);
			Levels = new Dictionary<int, AchievementLevel>();
		}

		public void Compose(ServerMessage message, IPlayer player)
		{
			int amount = 0;
			if (player.Achievement.TryGetAchievementProgress(Id, out IPlayerAchievement playerAchievement))
				amount = playerAchievement.Progress;

			TryGetLevelFromProgress(amount, out AchievementLevel currentLevel);
			TryGetLevel(currentLevel != null ? currentLevel.Level + 1 : 1, out AchievementLevel nextLevel);

			int targetLevel = nextLevel != null ? nextLevel.Level : currentLevel != null ? currentLevel.Level : 1;

			message.WriteInt(Id);
			message.WriteInt(targetLevel);
			message.WriteString("ACH_" + Name + targetLevel);
			message.WriteInt(currentLevel != null ? currentLevel.Progress : 0);
			message.WriteInt(nextLevel != null ? nextLevel.Progress : 0);
			message.WriteInt(nextLevel != null ? nextLevel.RewardAmount : 0);
			message.WriteInt(nextLevel != null ? nextLevel.RewardType : 0);
			message.WriteInt(amount);
			message.WriteBoolean(player.Achievement.HasAchieved(this));
			message.WriteString(Category.ToString().ToLower());
			message.WriteString(string.Empty); //dunno?
			message.WriteInt(Levels.Count);
			message.WriteInt(player.Achievement.HasAchieved(this) ? 1 : 0);
		}

		public bool TryGetLevelFromProgress(int progress, out AchievementLevel achievementLevel)
		{
			achievementLevel = null;

			if (progress > 0)
			{
				foreach (AchievementLevel level in Levels.Values)
				{
					if (progress >= level.Progress)
					{
						if (achievementLevel != null)
						{
							if (achievementLevel.Level > level.Level)
								continue;
						}

						achievementLevel = level;
					}
				}
			}

			return achievementLevel != null;
		}

		public bool TryGetLevel(int level, out AchievementLevel achievementLevel) =>
			Levels.TryGetValue(level, out achievementLevel);
	}
}
