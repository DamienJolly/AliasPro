using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Groups.Types;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Groups.Packets.Events
{
	public class GroupRemoveAdminEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.GroupRemoveAdminMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IRoomController _roomController;
		private readonly IPlayerController _playerController;

		public GroupRemoveAdminEvent(
			IGroupController groupController,
			IRoomController roomController,
			IPlayerController playerController)
		{
			_groupController = groupController;
			_roomController = roomController;
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

			member.Rank = GroupRank.MEMBER;
			await session.SendPacketAsync(new GroupMemberUpdateComposer(group, member));

			if (_roomController.TryGetRoom((uint)group.RoomId, out IRoom room))
			{
				if (_playerController.TryGetPlayer((uint)playerId, out IPlayer player))
					await room.Rights.ReloadRights(player.Session);
			}
		}
	}
}

