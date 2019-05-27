using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Groups.Packets.Events
{
	public class GroupDeclineMembershipEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.GroupDeclineMembershipMessageEvent;

		private readonly IGroupController _groupController;

		public GroupDeclineMembershipEvent(
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

			if (!group.IsAdmin((int)session.Player.Id)) return;

			if (!group.TryGetMember(userId, out IGroupMember member))
			{
				await session.SendPacketAsync(new GroupAcceptMemberErrorComposer(group.Id, GroupAcceptMemberErrorComposer.NO_LONGER_MEMBER));
				return;
			}

			group.RemoveMember(playerId);
			await _groupController.RemoveGroupMember(group.Id, playerId);
			// todo: eject furni
			await session.SendPacketAsync(new GroupRefreshMembersListComposer(group));
		}
	}
}

