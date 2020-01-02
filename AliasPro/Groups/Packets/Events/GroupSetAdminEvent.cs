using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Groups.Types;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Groups.Packets.Events
{
	public class GroupSetAdminEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.GroupSetAdminMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IPlayerController _playerController;

		public GroupSetAdminEvent(
			IGroupController groupController,
			IPlayerController playerController)
		{
			_groupController = groupController;
			_playerController = playerController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			int groupId = clientPacket.ReadInt();
			int playerId = clientPacket.ReadInt();

			IGroup group = await _groupController.ReadGroupData(groupId);
			if (group == null) return;

			if (!group.IsOwner((int)session.Player.Id)) return;

			if (!group.TryGetMember(playerId, out IGroupMember member))
				return;

			member.Rank = GroupRank.ADMIN;
			await session.SendPacketAsync(new GroupMemberUpdateComposer(group, member));

			if (_playerController.TryGetPlayer((uint)playerId, out IPlayer player))
			{
				IRoom room = player.Session.CurrentRoom;
				if (room != null && room.Group != null && room.Group.Id == groupId)
					await room.Rights.ReloadRights(player.Session);
			}
		}
	}
}

