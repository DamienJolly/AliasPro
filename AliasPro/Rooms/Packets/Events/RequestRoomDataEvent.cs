using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomDataEvent : IMessageEvent
    {
        public short Header => Incoming.RequestRoomDataMessageEvent;

        private readonly IRoomController _roomController;

        public RequestRoomDataEvent(
			IRoomController roomController)
        {
            _roomController = roomController;
        }

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
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
