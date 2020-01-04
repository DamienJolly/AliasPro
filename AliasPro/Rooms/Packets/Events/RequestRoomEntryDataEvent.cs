using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
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
            await session.SendPacketAsync(new FloorHeightMapComposer(room.WallHeight, room.RoomModel.RelativeHeightMap));
            
            if (session.Entity == null)
            {
                int entityId = room.Entities.NextEntitityId++;
                BaseEntity userEntity = new PlayerEntity(
                    entityId,
                    room.RoomModel.DoorX,
                    room.RoomModel.DoorY,
                    room.RoomModel.DoorDir,
                    session);
                
                session.Entity = userEntity;
                await room.AddEntity(userEntity);
            }

            await room.Rights.ReloadRights(session);

            await session.SendPacketAsync(new RoomEntryInfoComposer(room.Id, 
                room.Rights.HasRights(session.Player.Id)));

            await session.SendPacketAsync(new EntitiesComposer(room.Entities.Entities));
            await session.SendPacketAsync(new EntityUpdateComposer(room.Entities.Entities));

			foreach (BaseEntity botEntity in room.Entities.BotEntities)
			{
				if (botEntity.DanceId != 0)
					await session.SendPacketAsync(new UserDanceComposer(botEntity));
			}

			await session.SendPacketAsync(new RoomVisualizationSettingsComposer(room.Settings));

            await session.SendPacketAsync(new RoomFloorItemsComposer(room.Items.FloorItems, room.Items.GetItemOwners));
            await session.SendPacketAsync(new RoomWallItemsComposer(room.Items.WallItems, room.Items.GetItemOwners));
            await session.SendPacketAsync(new RoomPromotionComposer(room));

            await session.SendPacketAsync(new UpdateStackHeightComposer(room.RoomGrid.RoomTiles));
        }
    }
}
