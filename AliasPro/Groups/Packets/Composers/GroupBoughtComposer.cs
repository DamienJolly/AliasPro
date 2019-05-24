using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupBoughtComposer : IPacketComposer
	{
		private readonly IGroup _group;

		public GroupBoughtComposer(
			IGroup group)
		{
			_group = group;
		}

		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.GroupBoughtMessageComposer);
			message.WriteInt(_group.RoomId);
			message.WriteInt(_group.Id);
			return message;
		}
	}
}
