using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Badges.Packets.Composers
{
    public class AddPlayerBadgeComposer : IPacketComposer
    {
		private readonly IPlayerBadge _badge;

		public AddPlayerBadgeComposer(
			IPlayerBadge badge)
        {
			_badge = badge;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AddPlayerBadgeMessageComposer);
			message.WriteInt(1); //?
			message.WriteString(_badge.Code);
			return message;
        }
    }
}
