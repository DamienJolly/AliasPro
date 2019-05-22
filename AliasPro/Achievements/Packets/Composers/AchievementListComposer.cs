using AliasPro.API.Achievements.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Achievements.Packets.Composers
{
    public class AchievementListComposer : IPacketComposer
    {
        private readonly IPlayer _player;
		private readonly ICollection<IAchievement> _achievements;

		public AchievementListComposer(
			IPlayer player,
			ICollection<IAchievement> achievements)
        {
			_player = player;
			_achievements = achievements;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AchievementListMessageComposer);
			message.WriteInt(_achievements.Count);
			foreach (IAchievement achievement in _achievements)
			{
				int amount = 0;
				//if (!player.Achievements.GetAchievementProgress(achievement.Id, out int amount))
				//	amount = 0;

				int targetLevel = 1;
				IAchievementLevel currentLevel = achievement.GetLevelForProgress(amount);
				IAchievementLevel nextLevel = achievement.GetNextLevel(currentLevel != null ? currentLevel.Level : 0);

				message.WriteInt(achievement.Id);
				message.WriteInt(targetLevel);
				message.WriteString("ACH_" + achievement.Name + targetLevel);
				message.WriteInt(currentLevel != null ? currentLevel.Progress : 0);
				message.WriteInt(nextLevel != null ? nextLevel.Progress : 0);
				message.WriteInt(nextLevel != null ? nextLevel.RewardAmount : 0);
				message.WriteInt(nextLevel != null ? nextLevel.RewardType : 0);
				message.WriteInt(amount);
				message.WriteBoolean(false); //has achieved
				message.WriteString(achievement.Category.ToString().ToLower());
				message.WriteString(string.Empty); //dunno?
				message.WriteInt(achievement.Levels.Count);
				message.WriteInt(0); //1 = Progressbar visible if the achievement is completed
			}
			message.WriteString(string.Empty);
			return message;
        }
    }
}
