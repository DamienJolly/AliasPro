using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Groups.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Groups.Packets.Events
{
	public class RequestGroupBuyRoomsEvent : IMessageEvent
	{
		public short Id { get; } = Incoming.RequestGroupBuyRoomsMessageEvent;

		private readonly IRoomController _roomController;

		public RequestGroupBuyRoomsEvent(
			IRoomController roomController)
		{
			_roomController = roomController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
		{
			if (session.Player == null)
				return;

			ICollection<IRoomData> rooms = new List<IRoomData>();
			foreach (IRoomData room in 
				await _roomController.GetPlayersRooms(session.Player.Id))
			{
				if (room.OwnerId == session.Player.Id && room.Group == null)
					rooms.Add(room);
			}

			await session.SendPacketAsync(new GroupBuyRoomsComposer(rooms));
		}
	}
}

