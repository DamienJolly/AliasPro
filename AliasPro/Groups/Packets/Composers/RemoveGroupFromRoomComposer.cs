using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Groups.Packets.Composers
{
	public class RemoveGroupFromRoomComposer : IMessageComposer
	{
		private readonly int _groupId;

		public RemoveGroupFromRoomComposer(
			int groupId)
		{
			_groupId = groupId;
		}

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.RemoveGroupFromRoomMessageComposer);
			message.WriteInt(_groupId);
			return message;
		}
	}
}
