using AliasPro.API.Groups.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupFavoriteUpdateComposer : IMessageComposer
	{
		private readonly BaseEntity _entity;
		private readonly IGroup _group;

		public GroupFavoriteUpdateComposer(BaseEntity entity, IGroup group)
		{
			_entity = entity;
			_group = group;
		}

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.GroupFavoriteUpdateMessageComposer);
			message.WriteInt(_entity.Id);
			message.WriteInt(_group != null ? _group.Id : -1);
			message.WriteInt(_group != null ? (int)_group.State : -1);
			message.WriteString(_group != null ? _group.Name : string.Empty);
			return message;
		}
	}
}
