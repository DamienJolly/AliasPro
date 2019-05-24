using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Groups.Packets.Events
{
	public class RequestGroupMembersEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.RequestGroupMembersMessageEvent;

		private readonly IGroupController _groupController;

		public RequestGroupMembersEvent(
			IGroupController groupController)
		{
			_groupController = groupController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			int groupId = clientPacket.ReadInt();
			int pageId = clientPacket.ReadInt();
			string query = clientPacket.ReadString();
			int levelId = clientPacket.ReadInt();

			IGroup group = await _groupController.ReadGroupData(groupId);
			if (group == null) return;

			await session.SendPacketAsync(new GroupMembersComposer(session.Player, group, pageId, levelId, query));
		}
	}
}

