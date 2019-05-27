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
	public class GroupChangeColorsEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.GroupChangeColorsMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IRoomController _roomController;

		public GroupChangeColorsEvent(
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

			if (group == null)
				return;

			int colourOne = clientPacket.ReadInt();
			int colourTwo = clientPacket.ReadInt();

			//todo: maybe a check

			group.ColourOne = colourOne;
			group.ColourTwo = colourTwo;

			if (_roomController.TryGetRoom((uint)group.RoomId, out IRoom room))
				await room.UpdateRoomGroup(group);
		}
	}
}

