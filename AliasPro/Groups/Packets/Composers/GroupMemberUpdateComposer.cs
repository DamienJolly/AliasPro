using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupMemberUpdateComposer : IPacketComposer
	{
		private readonly IGroup _group;
		private readonly IGroupMember _member;

		public GroupMemberUpdateComposer(
			IGroup group,
			IGroupMember member)
		{
			_group = group;
			_member = member;
		}

		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.GroupMemberUpdateMessageComposer);
			message.WriteInt(_group.Id);
			_member.Compose(message);
			return message;
		}
	}
}
