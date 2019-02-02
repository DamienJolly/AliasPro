using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;

    public class RequestRoomDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomDataMessageEvent;

        private readonly IRoomController _roomController;

        public RequestRoomDataEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int roomId = clientPacket.ReadInt();
            IRoom room = await _roomController.GetRoomByIdAsync(roomId);
            if (room != null)
            {
                bool loading = !(clientPacket.ReadInt() == 0 && clientPacket.ReadInt() == 1);
                await session.SendPacketAsync(new RoomDataComposer(room, loading, true, session));
            }
        }
    }
}
