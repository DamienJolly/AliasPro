using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Models;
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

    public interface IRoomController
    {
        ICollection<IRoom> Rooms { get; }
        Task<ICollection<IRoomData>> GetPlayersRoomsAsync(uint playerId);
        Task<IRoomData> GetRoomDataAsync(uint roomId);
        bool TryAddRoom(IRoom room);
        void RemoveRoom(IRoom room);
        bool TryGetRoom(uint roomId, out IRoom room);

        bool TryGetRoomModel(string modelName, out IRoomModel model);

        Task<IRoomSettings> GetRoomSettingsAsync(uint roomId);
        Task CreateRoomSettingsAsync(uint roomId);

        Task<IDictionary<uint, string>> GetRightsForRoomAsync(uint roomId);
        Task GiveRoomRights(uint roomId, uint playerId);
        Task TakeRoomRights(uint roomId, uint playerId);
    }
}
