using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomDataMessageEvent;

        private readonly IRoomController _roomController;

        public RequestRoomDataEvent(
			IRoomController roomController)
        {
            _roomController = roomController;
        }

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			int roomId = clientPacket.ReadInt();
			IRoom room = await _roomController.LoadRoom((uint)roomId);

			if (room == null)
				return;

			bool loading = clientPacket.ReadInt() == 1;
			bool entry = clientPacket.ReadInt() == 1;

			await session.SendPacketAsync(new RoomDataComposer(room, loading, entry, session));
        }
    }
}
