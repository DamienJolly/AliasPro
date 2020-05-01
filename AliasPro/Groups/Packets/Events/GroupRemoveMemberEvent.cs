using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
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
		public short Header => Incoming.GroupRemoveMemberMessageEvent;

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
			ClientMessage message)
		{
			int groupId = message.ReadInt();
			int playerId = message.ReadInt();
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

			if (_playerController.TryGetPlayer((uint)playerId, out IPlayer player))
			{
				player.RemoveGroup(group.Id);
				await player.Session.SendPacketAsync(new GroupInfoComposer(group, player, false));

				if (player.IsFavoriteGroup(group.Id))
				{
					player.FavoriteGroup = 0;

					if (player.Session.CurrentRoom.Id == group.RoomId)
						await player.Session.CurrentRoom.SendPacketAsync(new GroupFavoriteUpdateComposer(player.Session.Entity, null));
				}

				if (player.Session.CurrentRoom.Id == group.RoomId)
				{
					await player.Session.CurrentRoom.Rights.ReloadRights(player.Session);
					await player.Session.CurrentRoom.SendPacketAsync(new GroupRefreshGroupsComposer(playerId));
				}
				else
					await player.Session.CurrentRoom.SendPacketAsync(new GroupRefreshGroupsComposer(playerId));

				//todo: eject furniture
			}

			group.RemoveMember(playerId);
			await _groupController.RemoveGroupMember(group.Id, playerId);
			await _playerController.RemoveFavoriteGroup(group.Id, playerId);

			if (playerId != session.Player.Id)
				await session.SendPacketAsync(new GroupRefreshMembersListComposer(group));
		}
	}
}

