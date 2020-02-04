using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Groups.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Groups.Packets.Events
{
	public class RequestGroupMembersEvent : IMessageEvent
	{
		public short Id { get; } = Incoming.RequestGroupMembersMessageEvent;

		private readonly IGroupController _groupController;

		public RequestGroupMembersEvent(
			IGroupController groupController)
		{
			_groupController = groupController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
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

