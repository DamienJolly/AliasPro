using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
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
	public class GroupRemoveMemberEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.GroupRemoveMemberMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IPlayerController _playerController;

		public GroupRemoveMemberEvent(
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

			if (playerId == session.Player.Id)
			{
				if (group.IsOwner(playerId)) return;
			}
			else
			{
				if (!group.IsMember(playerId)) return;

				if (group.IsAdmin(playerId) && 
					!group.IsOwner((int)session.Player.Id)) return;
			}

			group.RemoveMember(playerId);
			await _groupController.RemoveGroupMember(group.Id, playerId);
			// todo: eject furni

			if (playerId == session.Player.Id)
			{
				await session.SendPacketAsync(new GroupInfoComposer(group, session.Player, false));

				IRoom room = session.Player.Session.CurrentRoom;
				if (room != null && room.Group.Id == groupId)
					await room.Rights.ReloadRights(session);
			}
			else
			{
				if (_playerController.TryGetPlayer((uint)playerId, out IPlayer player))
				{
					await player.Session.SendPacketAsync(new GroupInfoComposer(group, player, false));

					IRoom room = player.Session.CurrentRoom;
					if (room != null && room.Group.Id == groupId)
						await room.Rights.ReloadRights(player.Session);
				}

				await session.SendPacketAsync(new GroupRefreshMembersListComposer(group));
			}
		}
	}
}

