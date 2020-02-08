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
	public class RequestGroupManageEvent : IMessageEvent
	{
		public short Header => Incoming.RequestGroupManageMessageEvent;

		private readonly IGroupController _groupController;

		public RequestGroupManageEvent(
			IGroupController groupController)
		{
			_groupController = groupController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
		{
			int groupId = clientPacket.ReadInt();
			IGroup group = await _groupController.ReadGroupData(groupId);

			if (group == null)
				return;

			await session.SendPacketAsync(new GroupManageComposer(group));
		}
	}
}

