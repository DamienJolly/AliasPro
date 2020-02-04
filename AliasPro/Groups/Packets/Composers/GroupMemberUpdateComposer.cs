using AliasPro.API.Groups.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupMemberUpdateComposer : IMessageComposer
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

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.GroupMemberUpdateMessageComposer);
			message.WriteInt(_group.Id);
			_member.Compose(message);
			return message;
		}
	}
}
