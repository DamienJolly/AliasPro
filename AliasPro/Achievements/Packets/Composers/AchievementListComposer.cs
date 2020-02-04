using AliasPro.API.Achievements.Models;
using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Achievements.Packets.Composers
{
    public class AchievementListComposer : IMessageComposer
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

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AchievementListMessageComposer);
			message.WriteInt(_achievements.Count);

			foreach (IAchievement achievement in _achievements)
				achievement.Compose(message, _player);

			message.WriteString(string.Empty); //dunno?
			return message;
        }
    }
}
