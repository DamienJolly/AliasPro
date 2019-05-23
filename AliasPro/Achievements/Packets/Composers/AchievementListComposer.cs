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
				achievement.Compose(message, _player);

			message.WriteString(string.Empty); //dunno?
			return message;
        }
    }
}
