using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;

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

			await session.SendPacketAsync(new GroupBuyRoomsComposer(
				await _roomController.GetPlayersRoomsAsync(session.Player.Id)));
		}
	}
}

