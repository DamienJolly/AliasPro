using AliasPro.API.Items;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Components;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Cycles;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomDataMessageEvent;

        private readonly IRoomController _roomController;
        private readonly IItemController _itemController;

        public RequestRoomDataEvent(IRoomController roomController, IItemController itemController)
        {
            _roomController = roomController;
            _itemController = itemController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint roomId = (uint)clientPacket.ReadInt();
            if (!_roomController.TryGetRoom(roomId, out IRoom room))
            {
                IRoomData roomData = await _roomController.ReadRoomDataAsync(roomId);
                if (roomData == null)
                    return;

                room = new Room(roomData);

                if (!_roomController.TryGetRoomModel(room.ModelName, out IRoomModel model))
                    return;

                if (!_roomController.TryAddRoom(room))
                    return;

                IRoomSettings roomSettings =
                        await _roomController.GetRoomSettingsAsync(room.Id);

                if (roomSettings == null)
                {
                    await _roomController.CreateRoomSettingsAsync(room.Id);
                    roomSettings =
                        await _roomController.GetRoomSettingsAsync(room.Id);
                }

                room.RoomModel = model;
                room.Settings = roomSettings;
                room.Entities = new EntitiesComponent(room);
                room.Game = new GameComponent(room);
                room.RoomGrid = new RoomGrid(room);

                room.Items = new ItemsComponent(
                    room,
                    await _itemController.GetItemsForRoomAsync(room.Id));

                room.Rights = new RightsComponent(room,
                    await _roomController.GetRightsForRoomAsync(room.Id));

                room.RoomCycle = new RoomCycle(room);
            }

            bool loading = !(clientPacket.ReadInt() == 0 && clientPacket.ReadInt() == 1);
            await session.SendPacketAsync(new RoomDataComposer(room, loading, true, session));
        }
    }
}
