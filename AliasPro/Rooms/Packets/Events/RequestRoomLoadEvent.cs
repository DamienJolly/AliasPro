﻿using AliasPro.API.Items;
using AliasPro.API.Messenger;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Components;
using AliasPro.Rooms.Cycles;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomLoadEvent : IMessageEvent
    {
        public short Header => Incoming.RequestRoomLoadMessageEvent;

        private readonly IRoomController _roomController;
        private readonly IItemController _itemController;
        private readonly IMessengerController _messengerController;

        public RequestRoomLoadEvent(
            IRoomController roomController, 
            IItemController itemController,
            IMessengerController messengerController)
        {
            _roomController = roomController;
            _itemController = itemController;
            _messengerController = messengerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            uint roomId = (uint)message.ReadInt();
            string password = message.ReadString();

            if (session.Entity != null)
                await session.CurrentRoom.RemoveEntity(session.Entity, false);

            IRoom room = await _roomController.LoadRoom((uint)roomId);

            if (room == null)
            {
                await session.SendPacketAsync(new RoomCloseComposer());
                return;
            }

            if (!room.Loaded)
            {
                if (!_roomController.TryGetRoomModel(room.ModelName, out IRoomModel model))
                {
                    await session.SendPacketAsync(new RoomCloseComposer());
                    return;
                }

                room.RoomModel = model;

                room.Entities = new EntitiesComponent(
                    room,
                    await _roomController.GetBotsForRoomAsync(room),
                    await _roomController.GetPetsForRoomAsync(room));

                room.Mute = new MuteComponent(room);

                room.Bans = new BanComponent(
                    room,
                    await _roomController.GetBannedPlayers(room.Id));

                room.Trax = new TraxComponent(
                    room,
                    await _roomController.GetTraxForRoomAsync(room.Id));

                room.RoomGrid = new RoomGrid(room);

                room.Items = new ItemsComponent(
                    room,
                    await _itemController.GetItemsForRoomAsync(room.Id));

                room.Rights = new RightsComponent(room,
                    await _roomController.GetRightsForRoomAsync(room.Id));

                room.WordFilter = await _roomController.GetWordFilterForRoomAsync(room.Id);

                room.RoomCycle = new RoomCycle(room);
                room.Loaded = true;
            }

            if (room.Bans.PlayerBanned((int)session.Player.Id))
            {
                await session.SendPacketAsync(new RoomEnterErrorComposer(RoomEnterErrorComposer.ROOM_ERROR_BANNED));
                return;
            }

            if (room.Entities.Entities.Count >= room.MaxUsers)
            {
                await session.SendPacketAsync(new RoomEnterErrorComposer(RoomEnterErrorComposer.ROOM_ERROR_GUESTROOM_FULL));
                return;
            }

            await session.SendPacketAsync(new RoomOpenComposer());

            if (!room.Rights.HasRights(session.Player.Id))
            {
                if (room.DoorState == 1)
                {
                    foreach (BaseEntity entity in room.Entities.Entities)
                    {
                        if (!(entity is PlayerEntity playerEntity)) continue;

                        if (!room.Rights.HasRights(playerEntity.Player.Id)) continue;

                        await playerEntity.Session.SendPacketAsync(new DoorbellAddUserComposer(session.Player.Username));
                    }

                    session.CurrentRoom = room;
                    await session.SendPacketAsync(new DoorbellAddUserComposer(string.Empty));
                    return;
                }
                else if (room.DoorState == 2)
                {
                    if (room.Password != password)
                    {
                        await session.SendPacketAsync(new GenericErrorComposer(GenericErrorComposer.ROOM_PASSWORD_INCORRECT));
                        await session.SendPacketAsync(new RoomCloseComposer());
                        return;
                    }
                }
            }

            session.CurrentRoom = room;

            await session.SendPacketAsync(new RoomModelComposer(room.RoomModel.Id, room.Id));
            await session.SendPacketAsync(new RoomScoreComposer(room.Score));

            if (!room.WallPaint.Equals("0.0"))
                await session.SendPacketAsync(new RoomPaintComposer("wallpaper", room.WallPaint));

            if (!room.FloorPaint.Equals("0.0"))
                await session.SendPacketAsync(new RoomPaintComposer("floor", room.FloorPaint));

            await session.SendPacketAsync(new RoomPaintComposer("landscape", room.BackgroundPaint));

            if (session.Player.Messenger != null)
                await _messengerController.UpdateStatusAsync(session.Player, session.Player.Messenger.Friends);
        }
    }
}
