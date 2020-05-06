using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Achievements.Models;

namespace AliasPro.Game.Achievements.Packets.Composers
{
    public class AchievementUnlockedComposer : IMessageComposer
    {
        private readonly IPlayer player;
		private readonly AchievementData achievement;
		private readonly AchievementLevel achievementLevel;

		public AchievementUnlockedComposer(
			IPlayer player,
			AchievementData achievement,
			AchievementLevel achievementLevel)
        {
			this.player = player;
			this.achievement = achievement;
			this.achievementLevel = achievementLevel;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AchievementUnlockedMessageComposer);
			message.WriteInt(achievement.Id);
			message.WriteInt(achievementLevel.Level);
			message.WriteInt(144);
			message.WriteString("ACH_" + achievement.Name + achievementLevel.Level);
			message.WriteInt(achievementLevel.RewardAmount);
			message.WriteInt(achievementLevel.RewardType);
			message.WriteInt(0);
			message.WriteInt(10);
			message.WriteInt(21);
			message.WriteString(achievementLevel.Level > 1 ? "ACH_" + achievement.Name + (achievementLevel.Level - 1) : "");
			message.WriteString(achievement.Category.ToString().ToLower());
			message.WriteBoolean(true);
			return message;
        }
    }
}
