using AliasPro.API.Groups.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupRefreshMembersListComposer : IMessageComposer
	{
		private readonly IGroup _group;

		public GroupRefreshMembersListComposer(
			IGroup group)
		{
			_group = group;
		}

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.GroupRefreshMembersListMessageComposer);
			message.WriteInt(_group.Id);
			message.WriteInt(0);
			return message;
		}
	}
}
