using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;
using System.Collections.Generic;

namespace AliasPro.Groups.Packets.Events
{
	public class RequestGroupBuyRoomsEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.RequestGroupBuyRoomsMessageEvent;

		private readonly IRoomController _roomController;

		public RequestGroupBuyRoomsEvent(
			IRoomController roomController)
		{
			_roomController = roomController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			if (session.Player == null)
				return;

			//todo: store in player class
			//_roomController.LoadPlayersRooms(session.Player.Id);
			ICollection<IRoom> rooms = new List<IRoom>();

			foreach (IRoom room in _roomController.Rooms)
			{
				if (room.OwnerId == session.Player.Id && room.Group == null)
					rooms.Add(room);
			}

			await session.SendPacketAsync(new GroupBuyRoomsComposer(rooms));
		}
	}
}

