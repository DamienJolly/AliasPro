using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;

    public class RequestRoomEntryDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomEntryDataMessageEvent;

        private readonly IRoomController _roomController;

        public RequestRoomEntryDataEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;

            await session.WriteAndFlushAsync(new HeightMapComposer(room.RoomModel));
            await session.WriteAndFlushAsync(new FloorHeightMapComposer(-1, room.RoomModel.RelativeHeightMap));
            
            //todo: add user to room

            await session.WriteAndFlushAsync(new RoomEntryInfoComposer(room.RoomData.Id, true));
            await session.WriteAndFlushAsync(new RoomVisualizationSettingsComposer(false, 0, 0));
        }
    }
}
