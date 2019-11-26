using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Badges.Packets.Composers
{
    public class AddPlayerBadgeComposer : IPacketComposer
    {
		private readonly int _badgeId;
		private readonly string _badgeCode;

		public AddPlayerBadgeComposer(
			int badgeId,
			string badgeCode)
        {
			_badgeId = badgeId;
			_badgeCode = badgeCode;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AddPlayerBadgeMessageComposer);
			message.WriteInt(_badgeId);
			message.WriteString(_badgeCode);
			return message;
        }
    }
}
