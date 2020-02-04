using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupRefreshGroupsComposer : IMessageComposer
	{
		private readonly int _playerId;

		public GroupRefreshGroupsComposer(
			int playerId)
		{
			_playerId = playerId;
		}

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.GroupRefreshGroupsMessageComposer);
			message.WriteInt(_playerId);
			return message;
		}
	}
}
