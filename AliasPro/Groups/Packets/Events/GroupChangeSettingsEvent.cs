using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Types;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Groups.Packets.Events
{
	public class GroupChangeSettingsEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.GroupChangeSettingsMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IRoomController _roomController;

		public GroupChangeSettingsEvent(
			IGroupController groupController,
			IRoomController roomController)
		{
			_groupController = groupController;
			_roomController = roomController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			int groupId = clientPacket.ReadInt();
			IGroup group = await _groupController.ReadGroupData(groupId);

			if (group == null) return;

			if (!group.IsOwner((int)session.Player.Id)) return;

			int state = clientPacket.ReadInt();
			bool rights = clientPacket.ReadInt() == 0 ? true : false;

			if (state < 0 || state > 2) return;

			group.State = (GroupState)state;

			//todo: group rights
			//group.Rights = rights;

			if (_roomController.TryGetRoom((uint)group.RoomId, out IRoom room))
				await room.UpdateRoomGroup(group);
		}
	}
}

