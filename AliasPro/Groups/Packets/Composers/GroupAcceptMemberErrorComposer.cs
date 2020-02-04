using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupAcceptMemberErrorComposer : IMessageComposer
	{
		public static readonly int NO_LONGER_MEMBER = 0;
		public static readonly int ALREADY_REJECTED = 1;
		public static readonly int ALREADY_ACCEPTED = 2;

		private readonly int _groupId;
		private readonly int _code;

		public GroupAcceptMemberErrorComposer(
			int groupId,
			int code)
		{
			_groupId = groupId;
			_code = code;
		}

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.GroupAcceptMemberErrorMessageComposer);
			message.WriteInt(_groupId);
			message.WriteInt(_code);
			return message;
		}
	}
}
