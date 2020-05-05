using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Game.Badges.Packets.Composers
{
    public class AddPlayerBadgeComposer : IMessageComposer
    {
		private readonly IPlayerBadge playerBadge;

		public AddPlayerBadgeComposer(
			IPlayerBadge playerBadge)
        {
			this.playerBadge = playerBadge;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AddPlayerBadgeMessageComposer);
			message.WriteInt(playerBadge.BadgeId);
			message.WriteString(playerBadge.Code);
			return message;
        }
    }
}
