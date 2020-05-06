namespace AliasPro.Game.Achievements.Models
{
	public class AchievementLevel
	{
		public int Level { get; }
		public int RewardAmount { get; }
		public int RewardType { get; }
		public int RewardPoints { get; }
		public int Progress { get; }

		public AchievementLevel(int level, int rewardAmount, int rewardType, int rewardPoints, int progress)
		{
			Level = level;
			RewardAmount = rewardAmount;
			RewardType = rewardType;
			RewardPoints = rewardPoints;
			Progress = progress;
		}
	}
}
