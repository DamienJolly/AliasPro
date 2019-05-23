using AliasPro.Achievements.Types;
using AliasPro.Achievements.Utilities;
using AliasPro.API.Achievements.Models;
using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using AliasPro.Network.Protocol;
using System.Collections.Generic;
using System.Data.Common;

namespace AliasPro.Achievements.Models
{
	internal class Achievement : IAchievement
	{
		internal Achievement(DbDataReader reader)
		{
			Id = reader.ReadData<int>("id");
			Name = reader.ReadData<string>("name");
			Category = AchievementCategoryUtility.GetCategory(
				reader.ReadData<string>("category"));
			Levels = new List<IAchievementLevel>();
		}

		public void Compose(ServerPacket message, IPlayer player)
		{
			int amount = 0;

			if (player.Achievement.GetAchievementProgress(Id, out IPlayerAchievement playerAchievement))
				amount = playerAchievement.Progress;

			int targetLevel = 1;
			bool hasAchieved = player.Achievement.HasAchieved(this);
			IAchievementLevel currentLevel = GetLevelForProgress(amount);
			IAchievementLevel nextLevel = GetNextLevel(currentLevel != null ? currentLevel.Level : 0);

			if (nextLevel != null)
				targetLevel = nextLevel.Level;

			if (currentLevel != null && currentLevel.Level == Levels.Count)
				targetLevel = currentLevel.Level;

			message.WriteInt(Id);
			message.WriteInt(targetLevel);
			message.WriteString("ACH_" + Name + targetLevel);
			message.WriteInt(currentLevel != null ? currentLevel.Progress : 0);
			message.WriteInt(nextLevel != null ? nextLevel.Progress : 0);
			message.WriteInt(nextLevel != null ? nextLevel.RewardAmount : 0);
			message.WriteInt(nextLevel != null ? nextLevel.RewardType : 0);
			message.WriteInt(amount);
			message.WriteBoolean(hasAchieved);
			message.WriteString(Category.ToString().ToLower());
			message.WriteString(string.Empty); //dunno?
			message.WriteInt(Levels.Count);
			message.WriteInt(hasAchieved ? 1 : 0);
		}

		public IAchievementLevel GetLevelForProgress(int progress)
		{
			IAchievementLevel levelToGo = null;

			if (progress > 0)
			{
				foreach (IAchievementLevel level in Levels)
				{
					if (progress >= level.Progress)
					{
						if (levelToGo != null)
						{
							if (levelToGo.Level > level.Level)
								continue;
						}

						levelToGo = level;
					}
				}
			}
			return levelToGo;
		}

		public IAchievementLevel GetNextLevel(int currentLevel)
		{
			foreach (IAchievementLevel level in Levels)
			{
				if (level.Level == (currentLevel + 1))
					return level;
			}

			return null;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public AchievementCategory Category { get; set; }
		public IList<IAchievementLevel> Levels { get; set; }
	}
}
