using AliasPro.API.Achievements.Models;
using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Achievements.Packets.Composers
{
    public class AchievementProgressComposer : IMessageComposer
    {
        private readonly IPlayer _player;
		private readonly IAchievement _achievement;

		public AchievementProgressComposer(
			IPlayer player,
			IAchievement achievement)
        {
			_player = player;
			_achievement = achievement;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AchievementProgressMessageComposer);
			_achievement.Compose(message, _player);
			return message;
        }
    }
}
