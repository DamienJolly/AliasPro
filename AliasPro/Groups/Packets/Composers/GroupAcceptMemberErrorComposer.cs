using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupAcceptMemberErrorComposer : IPacketComposer
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

		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.GroupAcceptMemberErrorMessageComposer);
			message.WriteInt(_groupId);
			message.WriteInt(_code);
			return message;
		}
	}
}
