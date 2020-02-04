using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Badges.Packets.Composers
{
    public class AddPlayerBadgeComposer : IMessageComposer
    {
		private readonly IPlayerBadge _playerBadge;

		public AddPlayerBadgeComposer(
			IPlayerBadge playerBadge)
        {
			_playerBadge = playerBadge;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AddPlayerBadgeMessageComposer);
			message.WriteInt(_playerBadge.BadgeId);
			message.WriteString(_playerBadge.Code);
			return message;
        }
    }
}
