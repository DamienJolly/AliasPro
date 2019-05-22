using AliasPro.Achievements.Types;
using AliasPro.Achievements.Utilities;
using AliasPro.API.Achievements.Models;
using AliasPro.API.Database;
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
