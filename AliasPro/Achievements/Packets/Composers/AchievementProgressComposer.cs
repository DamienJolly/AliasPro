using AliasPro.API.Achievements.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Achievements.Packets.Composers
{
    public class AchievementProgressComposer : IPacketComposer
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

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AchievementProgressMessageComposer);
			_achievement.Compose(message, _player);
			return message;
        }
    }
}
