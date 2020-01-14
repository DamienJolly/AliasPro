using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Groups.Packets.Events
{
	public class GroupSetFavoriteEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.GroupSetFavoriteMessageEvent;

		private readonly IGroupController _groupController;

		public GroupSetFavoriteEvent(
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

			if (!session.Player.HasGroup(group.Id))
				return;

			session.Player.FavoriteGroup = group.Id;

			if (session.CurrentRoom != null)
			{
				await session.CurrentRoom.SendAsync(new RoomGroupBadgesComposer(group));
				await session.CurrentRoom.SendAsync(new GroupRefreshGroupsComposer((int)session.Player.Id));
				await session.CurrentRoom.SendAsync(new GroupFavoriteUpdateComposer(session.Entity, group));
			}
			else
			{
				await session.SendPacketAsync(new GroupRefreshGroupsComposer((int)session.Player.Id));
			}
		}
	}
}

