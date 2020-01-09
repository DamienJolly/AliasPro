using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Groups.Packets.Events
{
	public class GroupRemoveFavoriteEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.GroupRemoveFavoriteMessageEvent;

		private readonly IGroupController _groupController;

		public GroupRemoveFavoriteEvent(
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

			session.Player.FavoriteGroup = 0;

			if (session.CurrentRoom != null)
			{
				await session.CurrentRoom.SendAsync(new GroupRefreshGroupsComposer((int)session.Player.Id));
				await session.CurrentRoom.SendAsync(new GroupFavoriteUpdateComposer(session.Entity, null));
			}
			else
			{
				await session.SendPacketAsync(new GroupRefreshGroupsComposer((int)session.Player.Id));
			}
		}
	}
}

