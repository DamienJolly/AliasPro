using AliasPro.Game.Achievements.Types;

namespace AliasPro.Game.Achievements.Utilities
{
	public class AchievementCategoryUtility
	{
		public static AchievementCategory GetCategory(string category)
		{
			switch (category.ToLower())
			{
				default:
				case "other":
					return AchievementCategory.OTHER;

				case "identity": 
					return AchievementCategory.IDENTITY;

				case "explore": 
					return AchievementCategory.EXPLORE;

				case "music": 
					return AchievementCategory.MUSIC;

				case "social":
					return AchievementCategory.SOCIAL;

				case "games":
					return AchievementCategory.GAMES;

				case "room_builder": 
					return AchievementCategory.ROOM_BUILDER;

				case "pets": 
					return AchievementCategory.PETS;

				case "tools": 
					return AchievementCategory.TOOLS;

				case "events": 
					return AchievementCategory.EVENTS;

				case "invisible": 
					return AchievementCategory.INVISIBLE;
			}
		}
	}
}
