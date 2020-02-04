using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Groups.Packets.Events
{
	public class GroupChangeBadgeEvent : IMessageEvent
	{
		public short Id { get; } = Incoming.GroupChangeBadgeMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IRoomController _roomController;

		public GroupChangeBadgeEvent(
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

			int count = clientPacket.ReadInt();

			string badge = "";
			for (int i = 0; i < count; i += 3)
			{
				int id = clientPacket.ReadInt();
				int colour = clientPacket.ReadInt();
				int pos = clientPacket.ReadInt();

				if (i == 0) badge += "b";
				else badge += "s";

				badge += (id < 100 ? "0" : "") + (id < 10 ? "0" : "") + id + (colour < 10 ? "0" : "") + colour + "" + pos;
			}

			group.Badge = badge;
			_groupController.BadgeImager.GenerateImage(group.Badge);

			if (_roomController.TryGetRoom((uint)group.RoomId, out IRoom room))
			{
				await room.UpdateRoomGroup(group);
				await room.SendPacketAsync(new RoomGroupBadgesComposer(group));
			}
		}
	}
}

