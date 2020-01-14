using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
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
	public class GroupDeleteEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.GroupDeleteMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IRoomController _roomController;
		private readonly IPlayerController _playerController;

		public GroupDeleteEvent(
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
			IGroup group = await _groupController.ReadGroupData(groupId);

			if (group == null) return;

			if (!group.IsOwner((int)session.Player.Id)) return;

			await _groupController.RemoveGroup(group.Id);

			if (_roomController.TryGetRoom((uint)group.RoomId, out IRoom room))
			{
				room.Group = null;
				await room.SendAsync(new RemoveGroupFromRoomComposer(group.Id));
			}

			foreach (IGroupMember member in group.Members.Values)
			{
				if (_playerController.TryGetPlayer((uint)member.PlayerId, out IPlayer player))
				{
					player.RemoveGroup(group.Id);

					if (session.CurrentRoom != null)
					{
						if (session.CurrentRoom == room)
							await session.CurrentRoom.Rights.ReloadRights(player.Session);

						await session.CurrentRoom.SendAsync(new GroupRefreshGroupsComposer((int)player.Id));
						await session.CurrentRoom.SendAsync(new GroupFavoriteUpdateComposer(session.Entity, null));
					}
					else
					{
						await player.Session.SendPacketAsync(new GroupRefreshGroupsComposer((int)player.Id));
					}
				}
			}
		}
	}
}

