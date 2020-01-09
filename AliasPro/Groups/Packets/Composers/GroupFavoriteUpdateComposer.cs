using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Entities;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupFavoriteUpdateComposer : IPacketComposer
	{
		private readonly BaseEntity _entity;
		private readonly IGroup _group;

		public GroupFavoriteUpdateComposer(BaseEntity entity, IGroup group)
		{
			_entity = entity;
			_group = group;
		}

		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.GroupFavoriteUpdateMessageComposer);
			message.WriteInt(_entity.Id);
			message.WriteInt(_group != null ? _group.Id : -1);
			message.WriteInt(_group != null ? (int)_group.State : -1);
			message.WriteString(_group != null ? _group.Name : string.Empty);
			return message;
		}
	}
}
