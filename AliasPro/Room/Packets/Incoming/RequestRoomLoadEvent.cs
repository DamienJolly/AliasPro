using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;

    public class RequestRoomLoadEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomLoadMessageEvent;

        private readonly IRoomController _roomController;

        public RequestRoomLoadEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int roomId = clientPacket.ReadInt();
            string password = clientPacket.ReadString();

            IRoom room = await _roomController.GetRoomByIdAndPassword(roomId, password);
            if (room != null)
            {
                await session.WriteAndFlushAsync(new RoomOpenComposer());
                await session.WriteAndFlushAsync(new RoomModelComposer(room.RoomModel.Id, room.RoomData.Id));
                await session.WriteAndFlushAsync(new RoomScoreComposer(room.RoomData.Score));

                session.CurrentRoom = room;
            }
            else
            {
                //todo: close connection to room
            }
        }
    }
}
