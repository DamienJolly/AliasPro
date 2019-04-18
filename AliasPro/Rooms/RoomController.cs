using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Rooms
{
    internal class RoomController : IRoomController
    {
        private readonly RoomRepository _roomRepository;

        public RoomController(RoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public ICollection<IRoom> Rooms =>
            _roomRepository.Rooms;

        public async Task<ICollection<IRoomData>> GetPlayersRoomsAsync(uint playerId) =>
            await _roomRepository.GetPlayersRoomsAsync(playerId);

        public async Task<IRoomData> GetRoomDataAsync(uint roomId) =>
            await _roomRepository.GetRoomDataAsync(roomId);

        public bool TryAddRoom(IRoom room) =>
            _roomRepository.TryAddRoom(room);

        public void RemoveRoom(IRoom room) =>
            _roomRepository.RemoveRoom(room);

        public bool TryGetRoom(uint roomId, out IRoom room) =>
            _roomRepository.TryGetRoom(roomId, out room);

        public async Task<int> CreateRoomAsync(uint playerId, string name, string description, string modelName, int categoryId, int maxUsers, int tradeType) =>
            await _roomRepository.CreateRoomAsync(playerId, name, description, modelName, categoryId, maxUsers, tradeType);


        public bool TryGetRoomModel(string modelName, out IRoomModel model) =>
            _roomRepository.TryGetRoomModel(modelName, out model);


        public async Task<IRoomSettings> GetRoomSettingsAsync(uint roomId) =>
            await _roomRepository.GetRoomSettingsAsync(roomId);

        public async Task CreateRoomSettingsAsync(uint roomId) =>
            await _roomRepository.CreateRoomSettingsAsync(roomId);


        public Task<IDictionary<uint, string>> GetRightsForRoomAsync(uint roomId) =>
           _roomRepository.GetRightsForRoomAsync(roomId);

        public Task GiveRoomRights(uint roomId, uint playerId) =>
            _roomRepository.CreateRoomRights(roomId, playerId);

        public Task TakeRoomRights(uint roomId, uint playerId) =>
            _roomRepository.RemoveRoomRights(roomId, playerId);
    }
}
