using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Achievements.Models;
using System.Collections.Generic;

namespace AliasPro.Game.Achievements.Packets.Composers
{
    public class AchievementListComposer : IMessageComposer
    {
        private readonly IPlayer player;
		private readonly ICollection<AchievementData> achievements;

		public AchievementListComposer(
			IPlayer player,
			ICollection<AchievementData> achievements)
        {
			this.player = player;
			this.achievements = achievements;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AchievementListMessageComposer);
			message.WriteInt(achievements.Count);

			foreach (AchievementData achievement in achievements)
				achievement.Compose(message, player);

			message.WriteString(string.Empty); //dunno?
			return message;
        }
    }
}
