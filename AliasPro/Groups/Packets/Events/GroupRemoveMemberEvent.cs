using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
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
	public class GroupRemoveMemberEvent : IMessageEvent
	{
		public short Id { get; } = Incoming.GroupRemoveMemberMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IPlayerController _playerController;

		public GroupRemoveMemberEvent(
			IGroupController groupController,
			IPlayerController playerController)
		{
			_groupController = groupController;
			_playerController = playerController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
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

			if (!_playerController.TryGetPlayer((uint)playerId, out IPlayer player)) return;

			player.RemoveGroup(group.Id);

			await player.Session.SendPacketAsync(new GroupInfoComposer(group, player, false));

			IRoom room = player.Session.CurrentRoom;
			if (room != null)
				await room.SendPacketAsync(new GroupRefreshGroupsComposer((int)player.Id));
			else
				await player.Session.SendPacketAsync(new GroupRefreshGroupsComposer((int)player.Id));

			if (player.IsFavoriteGroup(group.Id))
			{
				player.FavoriteGroup = 0;

				if (room != null)
					await room.SendPacketAsync(new GroupFavoriteUpdateComposer(player.Session.Entity, null));
			}

			if (room != null && room.Group != null && room.Group.Id == groupId)
				await room.Rights.ReloadRights(player.Session);

			// todo: eject furni

			group.RemoveMember(playerId);
			await _groupController.RemoveGroupMember(group.Id, playerId);
			await _playerController.RemoveFavoriteGroup(group.Id, playerId);

			if (playerId != session.Player.Id)
				await session.SendPacketAsync(new GroupRefreshMembersListComposer(group));
		}
	}
}

