using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Achievements.Models;

namespace AliasPro.Game.Achievements.Packets.Composers
{
    public class AchievementProgressComposer : IMessageComposer
    {
        private readonly IPlayer player;
		private readonly AchievementData achievement;

		public AchievementProgressComposer(
			IPlayer player,
			AchievementData achievement)
        {
			this.player = player;
			this.achievement = achievement;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AchievementProgressMessageComposer);
			achievement.Compose(message, player);
			return message;
        }
    }
}
