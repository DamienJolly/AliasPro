using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.Game.Achievements.Models;
using AliasPro.Game.Achievements.Packets.Composers;
using AliasPro.Players.Models;
using AliasPro.Players.Packets.Composers;
using System.Collections.Generic;

namespace AliasPro.Players.Components
{
    public class AchievementComponent
	{
        private readonly IDictionary<int, IPlayerAchievement> _achievements;

        public AchievementComponent(
            IDictionary<int, IPlayerAchievement> achievements)
        {
			_achievements = achievements;
        }

		public bool HasAchieved(AchievementData achievement)
		{
			if (!TryGetAchievementProgress(achievement.Id, out IPlayerAchievement playerAchievement))
				return false;

			if (!achievement.TryGetLevelFromProgress(playerAchievement.Progress, out AchievementLevel currentLevel))
				return false;

			return currentLevel.Level >= achievement.Levels.Count && playerAchievement.Progress >= currentLevel.Progress;
		}

		public async void ProgressAchievement(IPlayer player, AchievementData achievement, int amount = 1)
		{
			if (!TryGetAchievementProgress(achievement.Id, out IPlayerAchievement playerAchievement))
			{
				playerAchievement = new PlayerAchievement(achievement.Id, 0);
				player.Achievement.AddAchievement(playerAchievement);
				await Program.GetService<IPlayerController>().AddPlayerAchievementAsync(playerAchievement.Id, playerAchievement.Progress, player.Id);
			}

			if (achievement.TryGetLevelFromProgress(playerAchievement.Progress, out AchievementLevel oldLevel))
			{
				//Maximum achievement gotten.
				if (oldLevel.Level >= achievement.Levels.Count && amount >= oldLevel.Progress)
					return;
			}

			playerAchievement.Progress += amount;
			await Program.GetService<IPlayerController>().UpdatePlayerAchievementAsync(achievement.Id, amount, player.Id);
			await player.Session.SendPacketAsync(new AchievementProgressComposer(player, achievement));

			if (!achievement.TryGetLevelFromProgress(playerAchievement.Progress + amount, out AchievementLevel newLevel))
				return;

			if (oldLevel != null && oldLevel.Level == newLevel.Level && newLevel.Level < achievement.Levels.Count)
				return;

			await player.Session.SendPacketAsync(new AchievementUnlockedComposer(player, achievement, newLevel));

			IPlayerBadge badge = null;

			if (oldLevel != null)
			{
				if (player.Badge.TryGetBadge("ACH_" + achievement.Name + oldLevel.Level, out badge))
				{
					Program.GetService<IPlayerController>().UpdatePlayerBadge(player, badge, "ACH_" + achievement.Name + newLevel.Level);

					if (badge.Slot > 0)
					{
						if (player.Session.CurrentRoom != null)
							await player.Session.CurrentRoom.SendPacketAsync(new UserBadgesComposer( player.Badge.WornBadges, (int)player.Id));
					}
				}
			}

			//todo: badge
			//if (badge == null)
				//Program.GetService<IPlayerController>().AddPlayerBadge(player, "ACH_" + achievement.Name + newLevel.Level);

			player.Score += newLevel.RewardPoints;

			//if (player.Session.CurrentRoom != null)
				//await player.Session.SendPacketAsync(new RoomUserDataComposer(_player.Entity));
		}

		public void AddAchievement(IPlayerAchievement achievement) =>
			_achievements.Add(achievement.Id, achievement);

		public bool TryGetAchievementProgress(int id, out IPlayerAchievement achievement) => 
			_achievements.TryGetValue(id, out achievement);
	}
}
