using AliasPro.Achievements.Packets.Composers;
using AliasPro.API.Achievements;
using AliasPro.API.Achievements.Models;
using AliasPro.API.Players.Models;
using AliasPro.Players.Models;
using AliasPro.Players.Packets.Composers;
using System.Collections.Generic;

namespace AliasPro.Achievements
{
    internal class AchievementController : IAchievementController
	{
		private readonly AchievementDao _achievementDao;

		private IDictionary<string, IAchievement> _achievements;

		public AchievementController(AchievementDao achievementDao)
        {
			_achievementDao = achievementDao;

			_achievements = new Dictionary<string, IAchievement>();

			InitializeAchievements();
		}

		public async void InitializeAchievements()
		{
			_achievements = await _achievementDao.ReadAchievements();
		}

		public ICollection<IAchievement> Achievements =>
			_achievements.Values;

		public bool TryGetAchievement(string name, out IAchievement achievement) =>
			_achievements.TryGetValue(name, out achievement);

		public async void ProgressAchievement(IPlayer player, string name, int amount)
		{
			if (!TryGetAchievement(name, out IAchievement achievement))
				return;

			if (!player.Achievement.GetAchievementProgress(achievement.Id, out IPlayerAchievement playerAchievement))
			{
				playerAchievement = new PlayerAchievement(achievement.Id, amount);
				player.Achievement.AddAchievement(playerAchievement);
				await _achievementDao.AddPlayerAchievementAsync(achievement.Id, amount, player.Id);
			}

			IAchievementLevel oldLevel = achievement.GetLevelForProgress(playerAchievement.Progress);

			if (oldLevel == null)
				return;

			if (oldLevel.Level == achievement.Levels.Count && amount == oldLevel.Progress) //Maximum achievement reached.
				return;

			playerAchievement.Progress += amount;
			await _achievementDao.UpdatePlayerAchievementAsync(achievement.Id, amount, player.Id);
			await player.Session.SendPacketAsync(new AchievementProgressComposer(player, achievement));

			IAchievementLevel newLevel = achievement.GetLevelForProgress(playerAchievement.Progress + amount);
			if (!(oldLevel.Level != newLevel.Level && newLevel.Level >= achievement.Levels.Count))
				return;

			if (newLevel == null ||
				(oldLevel != null && oldLevel.Level == newLevel.Level && newLevel.Level < achievement.Levels.Count))
				return;

			await player.Session.SendPacketAsync(new AchievementUnlockedComposer(player, achievement));

			if (!player.Badge.TryGetBadge("ACH_" + achievement.Name + oldLevel.Level, out IPlayerBadge badge))
			{
				//todo: code
				//await player.Badge.AddBadgeAsync("ACH_" + achievement.Name + newLevel.Level);
			}
			else
			{
				// todo: code
				badge.Code = "ACH_" + achievement.Name + newLevel.Level;
				//await _player.Badges.UpdateBadgeAsync(badge, "ACH_" + achievement.Name + oldLevel.Level);
			}

			if (badge.Slot > 0)
			{
				if (player.Session.CurrentRoom != null)
					await player.Session.CurrentRoom.SendAsync(new UserBadgesComposer(
						player.Badge.WornBadges, player.Id));
				else
					await player.Session.SendPacketAsync(new UserBadgesComposer(
						player.Badge.WornBadges, player.Id));
			}

			player.Score += newLevel.RewardPoints;

			//if (player.Session.CurrentRoom != null)
				//await player.Session.SendPacketAsync(new RoomUserDataComposer(_player.Entity));

			//todo: talent track shit
		}
	}
}
