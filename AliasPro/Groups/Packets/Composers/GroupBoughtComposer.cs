using AliasPro.API.Groups.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupBoughtComposer : IMessageComposer
	{
		private readonly IGroup _group;

		public GroupBoughtComposer(
			IGroup group)
		{
			_group = group;
		}

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.GroupBoughtMessageComposer);
			message.WriteInt(_group.RoomId);
			message.WriteInt(_group.Id);
			return message;
		}
	}
}
