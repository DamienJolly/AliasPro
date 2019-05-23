using AliasPro.API.Achievements.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Achievements.Packets.Composers
{
    public class AchievementUnlockedComposer : IPacketComposer
    {
        private readonly IPlayer _player;
		private readonly IAchievement _achievement;

		public AchievementUnlockedComposer(
			IPlayer player,
			IAchievement achievement)
        {
			_player = player;
			_achievement = achievement;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AchievementUnlockedMessageComposer);
			int amount = 0;

			if (_player.Achievement.GetAchievementProgress(_achievement.Id, out IPlayerAchievement playerAchievement))
				amount = playerAchievement.Progress;

			IAchievementLevel level = _achievement.GetLevelForProgress(amount);
			message.WriteInt(_achievement.Id);
			message.WriteInt(level.Level);
			message.WriteInt(144);
			message.WriteString("ACH_" + _achievement.Name + level.Level);
			message.WriteInt(level.RewardAmount);
			message.WriteInt(level.RewardType);
			message.WriteInt(0);
			message.WriteInt(10);
			message.WriteInt(21);
			message.WriteString(level.Level > 1 ? "ACH_" + _achievement.Name + (level.Level - 1) : "");
			message.WriteString(_achievement.Category.ToString().ToLower());
			message.WriteBoolean(true);
			return message;
        }
    }
}
