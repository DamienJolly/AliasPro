using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Groups.Types;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Groups.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Groups.Types;
using AliasPro.Utilities;
using System.Threading.Tasks;

namespace AliasPro.Groups.Packets.Events
{
	public class RequestGroupJoinEvent : IMessageEvent
	{
		public short Id { get; } = Incoming.RequestGroupJoinMessageEvent;

		private readonly IGroupController _groupController;

		public RequestGroupJoinEvent(
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

			IRoom room = session.CurrentRoom;
			if (room != null)
				await room.SendPacketAsync(new GroupRefreshGroupsComposer((int)session.Player.Id));
			else
				await session.SendPacketAsync(new GroupRefreshGroupsComposer((int)session.Player.Id));

			if (group.State == GroupState.OPEN)
			{
				if (!session.Player.HasGroup(group.Id))
					session.Player.AddGroup(group.Id);

				if (room != null && room.Group != null && room.Group.Id == groupId)
					await room.Rights.ReloadRights(session);
			}
		}
	}
}

