using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Models;
using AliasPro.Room.Models.Entities;
using AliasPro.Room.Packets.Composers;

namespace AliasPro.Room.Packets.Events
{
    public class RequestRoomEntryDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomEntryDataMessageEvent;

        private readonly IRoomController _roomController;

        public RequestRoomEntryDataEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

            await session.SendPacketAsync(new HeightMapComposer(room.RoomModel));
            await session.SendPacketAsync(new FloorHeightMapComposer(-1, room.RoomModel.RelativeHeightMap));
            
            if (session.Entity == null)
            {
                int entityId = room.EntityHandler.NextEntitityId++;
                UserEntity userEntity = new UserEntity(
                    entityId,
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

            await session.SendPacketAsync(new RoomFloorItemsComposer(room.ItemHandler.FloorItems, room.ItemHandler.GetItemOwners));
            await session.SendPacketAsync(new RoomWallItemsComposer(room.ItemHandler.WallItems, room.ItemHandler.GetItemOwners));

            await room.RightHandler.ReloadRights(session);
        }
    }
}
