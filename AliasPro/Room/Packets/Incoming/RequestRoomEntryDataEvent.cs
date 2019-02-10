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
            if (room == null) return;

            await session.SendPacketAsync(new HeightMapComposer(room.RoomModel));
            await session.SendPacketAsync(new FloorHeightMapComposer(-1, room.RoomModel.RelativeHeightMap));

            if (session.Entity == null)
            {
                UserEntity userEntity = new UserEntity(
                    room.EntityHandler.Entities.Count + 1,
                    room.RoomModel.DoorX,
                    room.RoomModel.DoorY,
                    room.RoomModel.DoorDir,
                    session);

                session.Entity = userEntity;
                await room.AddEntity(userEntity);
            }
            
            await session.SendPacketAsync(new RoomEntryInfoComposer(room.RoomData.Id, 
                room.RightHandler.HasRights(session.Player.Id)));

            await session.SendPacketAsync(new EntitiesComposer(room.EntityHandler.Entities));
            await session.SendPacketAsync(new EntityUpdateComposer(room.EntityHandler.Entities));

            await session.SendPacketAsync(new RoomVisualizationSettingsComposer(room.RoomData.Settings));

            await session.SendPacketAsync(new RoomFloorItemsComposer(room.ItemHandler.FloorItems));
            await session.SendPacketAsync(new RoomWallItemsComposer(room.ItemHandler.WallItems));

            await room.RightHandler.ReloadRights(session);
        }
    }
}
