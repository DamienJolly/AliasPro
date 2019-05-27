using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Groups.Types;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Groups.Packets.Events
{
	public class GroupSetAdminEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.GroupSetAdminMessageEvent;

		private readonly IGroupController _groupController;

		public GroupSetAdminEvent(
			IGroupController groupController)
		{
			_groupController = groupController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			int groupId = clientPacket.ReadInt();
			int userId = clientPacket.ReadInt();

			IGroup group = await _groupController.ReadGroupData(groupId);
			if (group == null) return;

			if (group.OwnerId != session.Player.Id) return;

			if (!group.TryGetMember(userId, out IGroupMember member))
				return;

			member.Rank = GroupRank.ADMIN;
			await session.SendPacketAsync(new GroupMemberUpdateComposer(group, member));
		}
	}
}

