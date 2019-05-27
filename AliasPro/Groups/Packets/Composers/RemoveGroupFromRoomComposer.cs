using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Groups.Packets.Composers
{
	public class RemoveGroupFromRoomComposer : IPacketComposer
	{
		private readonly int _groupId;

		public RemoveGroupFromRoomComposer(
			int groupId)
		{
			_groupId = groupId;
		}

		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.RemoveGroupFromRoomMessageComposer);
			message.WriteInt(_groupId);
			return message;
		}
	}
}
