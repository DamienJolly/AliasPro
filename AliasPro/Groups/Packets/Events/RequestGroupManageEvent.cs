using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Groups.Packets.Events
{
	public class RequestGroupManageEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.RequestGroupManageMessageEvent;

		private readonly IGroupController _groupController;

		public RequestGroupManageEvent(
			IGroupController groupController)
		{
			_groupController = groupController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			int groupId = clientPacket.ReadInt();
			IGroup group = await _groupController.ReadGroupData(groupId);

			if (group == null)
				return;

			await session.SendPacketAsync(new GroupManageComposer(group));
		}
	}
}

