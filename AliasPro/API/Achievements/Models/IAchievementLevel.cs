namespace AliasPro.API.Achievements.Models
{
	public interface IAchievementLevel
	{
		int Level { get; set; }
		int RewardAmount { get; set; }
		int RewardType { get; set; }
		int RewardPoints { get; set; }
		int Progress { get; set; }
	}
}
