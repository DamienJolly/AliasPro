using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Groups.Types;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Groups.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Groups.Packets.Events
{
	public class GroupAcceptMembershipEvent : IMessageEvent
	{
		public short Header => Incoming.GroupAcceptMembershipMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IPlayerController _playerController;

		public GroupAcceptMembershipEvent(
			IGroupController groupController,
			IPlayerController playerController)
		{
			_groupController = groupController;
			_playerController = playerController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
		{
			int groupId = message.ReadInt();
			int playerId = message.ReadInt();

			IGroup group = await _groupController.ReadGroupData(groupId);
			if (group == null) return;

			if (!group.IsAdmin((int)session.Player.Id)) return;

			if (!group.TryGetMember(playerId, out IGroupMember member))
			{
				await session.SendPacketAsync(new GroupAcceptMemberErrorComposer(group.Id, GroupAcceptMemberErrorComposer.NO_LONGER_MEMBER));
				return;
			}

			if (member.Rank != GroupRank.REQUESTED)
			{
				await session.SendPacketAsync(new GroupAcceptMemberErrorComposer(group.Id, GroupAcceptMemberErrorComposer.ALREADY_ACCEPTED));
				return;
			}

			member.Rank = GroupRank.MEMBER;
			await session.SendPacketAsync(new GroupRefreshMembersListComposer(group));

			if (_playerController.TryGetPlayer((uint)playerId, out IPlayer player))
			{
				if (!player.HasGroup(group.Id))
					player.AddGroup(group.Id);

				IRoom room = player.Session.CurrentRoom;
				if (room != null && room.Group != null && room.Group.Id == groupId)
					await room.Rights.ReloadRights(player.Session);
			}
		}
	}
}

