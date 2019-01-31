using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;
    using Models.Entities;

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

            BaseEntity userEntity = _roomController.AddUserToRoom(room, session);

            await session.WriteAndFlushAsync(new RoomEntryInfoComposer(room.RoomData.Id, true));
            await session.WriteAndFlushAsync(new EntitiesComposer(room.Entities.Values));
            await session.WriteAndFlushAsync(new EntityUpdateComposer(room.Entities.Values));

            await session.WriteAndFlushAsync(new RoomVisualizationSettingsComposer(false, 0, 0));
        }
    }
}
