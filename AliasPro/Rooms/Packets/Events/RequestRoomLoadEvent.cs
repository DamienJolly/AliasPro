using AliasPro.API.Items;
using AliasPro.API.Messenger;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Components;
using AliasPro.Rooms.Cycles;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomLoadEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomLoadMessageEvent;

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

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint roomId = (uint)clientPacket.ReadInt();
            string password = clientPacket.ReadString();

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

                room.Game = new GameComponent(room);
                room.Mute = new MuteComponent(room);

                room.Bans = new BanComponent(
                    room,
                    await _roomController.GetBannedPlayers(room.Id));

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

            if (room.Password != password)
            {
                await session.SendPacketAsync(new GenericErrorComposer(GenericErrorComposer.ROOM_PASSWORD_INCORRECT));
                await session.SendPacketAsync(new RoomCloseComposer());
                return;
            }

            session.CurrentRoom = room;

            await session.SendPacketAsync(new RoomOpenComposer());
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
