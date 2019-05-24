using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Groups.Packets.Events
{
	public class RequestGroupInfoEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.RequestGroupInfoMessageEvent;

		private readonly IGroupController _groupController;

		public RequestGroupInfoEvent(
			IGroupController groupController)
		{
			_groupController = groupController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			if (session.Player == null)
				return;

			int groupId = clientPacket.ReadInt();
			bool newWindow = clientPacket.ReadBool();

			IGroup group = await _groupController.ReadGroupData(groupId);
			if (group == null)
				return;

			await session.SendPacketAsync(new GroupInfoComposer(group, session.Player, newWindow));
		}
	}
}

