﻿using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomEntryDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomEntryDataMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

            await session.SendPacketAsync(new HeightMapComposer(room));
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

            IDictionary<int, IGroup> groups = new Dictionary<int, IGroup>();
            foreach (BaseEntity entity in room.Entities.Entities)
			{
				if (entity.DanceId != 0)
					await session.SendPacketAsync(new UserDanceComposer(entity));

                //todo: handitems

                //todo: roomeffts

                if (entity.IsIdle)
                    await session.SendPacketAsync(new UserSleepComposer(entity));

                if (entity is PlayerEntity playerEntity)
                {
                    if ( playerEntity.Player.TryGetGroup(playerEntity.Player.FavoriteGroup, out IGroup group))
                    {
                        if (!groups.ContainsKey(group.Id))
                            groups.Add(group.Id, group);
                    }
                }
			}

            await session.SendPacketAsync(new RoomGroupBadgesComposer(groups.Values));

            await session.SendPacketAsync(new RoomVisualizationSettingsComposer(room.Settings));

            await session.SendPacketAsync(new RoomFloorItemsComposer(room.Items.FloorItems, room.Items.GetItemOwners));
            await session.SendPacketAsync(new RoomWallItemsComposer(room.Items.WallItems, room.Items.GetItemOwners));
            await session.SendPacketAsync(new RoomPromotionComposer(room));
        }
    }
}
