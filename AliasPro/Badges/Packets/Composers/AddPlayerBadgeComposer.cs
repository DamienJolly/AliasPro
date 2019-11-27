using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Badges.Packets.Composers
{
    public class AddPlayerBadgeComposer : IPacketComposer
    {
		private readonly IPlayerBadge _playerBadge;

		public AddPlayerBadgeComposer(
			IPlayerBadge playerBadge)
        {
			_playerBadge = playerBadge;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AddPlayerBadgeMessageComposer);
			message.WriteInt(_playerBadge.BadgeId);
			message.WriteString(_playerBadge.Code);
			return message;
        }
    }
}
