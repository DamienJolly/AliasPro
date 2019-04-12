using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;
using AliasPro.Room.Packets.Composers;
using AliasPro.Sessions;

namespace AliasPro.Room.Packets.Events
{
    public class RequestRoomDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomDataMessageEvent;

        private readonly IRoomController _roomController;

        public RequestRoomDataEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint roomId = (uint)clientPacket.ReadInt();
            IRoom room = await _roomController.GetRoomByIdAsync(roomId);
            if (room != null)
            {
                bool loading = !(clientPacket.ReadInt() == 0 && clientPacket.ReadInt() == 1);
                await session.SendPacketAsync(new RoomDataComposer(room, loading, true, session));
            }
        }
    }
}
