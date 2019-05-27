using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Groups.Packets.Events
{
	public class GroupChangeNameDescEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.GroupChangeNameDescMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IRoomController _roomController;

		public GroupChangeNameDescEvent(
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

			string name = clientPacket.ReadString();
			string description = clientPacket.ReadString();

			if (name.Length <= 0) return;

			group.Name = name;
			group.Description = description;

			if (_roomController.TryGetRoom((uint)group.RoomId, out IRoom room))
				await room.UpdateRoomGroup(group);
		}
	}
}

