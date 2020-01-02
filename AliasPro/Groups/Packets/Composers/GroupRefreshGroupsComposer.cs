using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupRefreshGroupsComposer : IPacketComposer
	{
		private readonly int _playerId;

		public GroupRefreshGroupsComposer(
			int playerId)
		{
			_playerId = playerId;
		}

		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.GroupRefreshGroupsMessageComposer);
			message.WriteInt(_playerId);
			return message;
		}
	}
}
