using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Groups.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Groups.Packets.Events
{
	public class GroupRemoveFavoriteEvent : IMessageEvent
	{
		public short Header => Incoming.GroupRemoveFavoriteMessageEvent;

		private readonly IGroupController _groupController;

		public GroupRemoveFavoriteEvent(
			IGroupController groupController)
		{
			_groupController = groupController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
		{
			int groupId = message.ReadInt();

			IGroup group = await _groupController.ReadGroupData(groupId);
			if (group == null) return;

			if (!session.Player.HasGroup(group.Id))
				return;

			session.Player.FavoriteGroup = 0;

			if (session.CurrentRoom != null)
			{
				await session.CurrentRoom.SendPacketAsync(new GroupRefreshGroupsComposer((int)session.Player.Id));
				await session.CurrentRoom.SendPacketAsync(new GroupFavoriteUpdateComposer(session.Entity, null));
			}
			else
			{
				await session.SendPacketAsync(new GroupRefreshGroupsComposer((int)session.Player.Id));
			}
		}
	}
}

