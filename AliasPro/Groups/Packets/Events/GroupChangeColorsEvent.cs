using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Groups.Packets.Events
{
	public class GroupChangeColorsEvent : IMessageEvent
	{
		public short Header => Incoming.GroupChangeColorsMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IRoomController _roomController;

		public GroupChangeColorsEvent(
			IGroupController groupController,
			IRoomController roomController)
		{
			_groupController = groupController;
			_roomController = roomController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
		{
			int groupId = clientPacket.ReadInt();
			IGroup group = await _groupController.ReadGroupData(groupId);

			if (group == null) return;

			if (!group.IsOwner((int)session.Player.Id)) return;

			int colourOne = clientPacket.ReadInt();
			int colourTwo = clientPacket.ReadInt();

			group.ColourOne = colourOne;
			group.ColourTwo = colourTwo;

			if (_roomController.TryGetRoom((uint)group.RoomId, out IRoom room))
				await room.UpdateRoomGroup(group);
		}
	}
}

