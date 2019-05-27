using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Groups.Types;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Groups.Types;
using AliasPro.Network.Events.Headers;
using AliasPro.Utilities;

namespace AliasPro.Groups.Packets.Events
{
	public class RequestGroupJoinEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.RequestGroupJoinMessageEvent;

		private readonly IGroupController _groupController;

		public RequestGroupJoinEvent(
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

			if (group == null) return;

			if (group.State == GroupState.CLOSED)
			{
				await session.SendPacketAsync(new GroupJoinErrorComposer(GroupJoinErrorComposer.GROUP_CLOSED));
				return;
			}

			if (group.State == GroupState.LOCKED && group.GetRequests >= 100)
			{
				await session.SendPacketAsync(new GroupJoinErrorComposer(GroupJoinErrorComposer.GROUP_NOT_ACCEPT_REQUESTS));
				return;
			}

			if (group.GetMembers >= 50000)
			{
				await session.SendPacketAsync(new GroupJoinErrorComposer(GroupJoinErrorComposer.GROUP_FULL));
				return;
			}

			int groupsCount = 0; //todo: player group count
			if (groupsCount >= 100)
			{
				await session.SendPacketAsync(new GroupJoinErrorComposer(GroupJoinErrorComposer.GROUP_LIMIT_EXCEED));
				return;
			}

			IGroupMember member = new GroupMember(
				(int)session.Player.Id, 
				session.Player.Username, 
				session.Player.Figure, 
				(int)UnixTimestamp.Now,
				group.State == GroupState.LOCKED ? GroupRank.REQUESTED : GroupRank.MEMBER);

			if (!group.TryAddMember(member)) return;

			await _groupController.AddGroupMember(group.Id, member);
			await session.SendPacketAsync(new GroupInfoComposer(group, session.Player, false));
		}
	}
}

