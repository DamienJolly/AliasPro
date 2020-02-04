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
	public class RequestGroupInfoEvent : IMessageEvent
	{
		public short Id { get; } = Incoming.RequestGroupInfoMessageEvent;

		private readonly IGroupController _groupController;

		public RequestGroupInfoEvent(
			IGroupController groupController)
		{
			_groupController = groupController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
		{
			if (session.Player == null)
				return;

			int groupId = clientPacket.ReadInt();
			bool newWindow = clientPacket.ReadBoolean();

			IGroup group = await _groupController.ReadGroupData(groupId);
			if (group == null)
				return;

			await session.SendPacketAsync(new GroupInfoComposer(group, session.Player, newWindow));
		}
	}
}

