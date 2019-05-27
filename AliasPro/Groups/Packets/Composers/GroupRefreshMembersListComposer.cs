using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupRefreshMembersListComposer : IPacketComposer
	{
		private readonly IGroup _group;

		public GroupRefreshMembersListComposer(
			IGroup group)
		{
			_group = group;
		}

		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.GroupRefreshMembersListMessageComposer);
			message.WriteInt(_group.Id);
			message.WriteInt(0);
			return message;
		}
	}
}
