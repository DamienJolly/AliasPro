using AliasPro.API.Achievements.Models;
using AliasPro.API.Database;
using System.Data.Common;

namespace AliasPro.Achievements.Models
{
	internal class AchievementLevel : IAchievementLevel
	{
		internal AchievementLevel(DbDataReader reader)
		{
			Level = reader.ReadData<int>("level");
			RewardAmount = reader.ReadData<int>("reward_amount");
			RewardType = reader.ReadData<int>("reward_type");
			RewardPoints = reader.ReadData<int>("reward_points");
			Progress = reader.ReadData<int>("progress");
		}

		public int Level { get; set; }
		public int RewardAmount { get; set; }
		public int RewardType { get; set; }
		public int RewardPoints { get; set; }
		public int Progress { get; set; }
	}
}
