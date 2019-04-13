using System.Threading.Tasks;

namespace AliasPro.Room
{
    using AliasPro.API.Sessions.Models;
    using Models;
    using System.Collections.Generic;

    internal class RoomController : IRoomController
    {
        private readonly RoomRepository _roomRepository;

        public RoomController(RoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public Task<int> AddNewRoomAsync(IRoomData roomData) =>
            _roomRepository.CreateRoom(roomData);

        public Task GiveRoomRights(uint roomId, uint playerId) =>
            _roomRepository.CreateRoomRights(roomId, playerId);

        public Task TakeRoomRights(uint roomId, uint playerId) =>
            _roomRepository.RemoveRoomRights(roomId, playerId);

        public Task<IDictionary<uint, string>> GetRightsForRoomAsync(uint roomId) =>
            _roomRepository.GetRightsForRoom(roomId);
        
        public Task<IRoom> GetRoomByIdAndPassword(uint id, string password) =>
            _roomRepository.GetRoomByIdAndPassword(id, password);

        public Task<IRoom> GetRoomByIdAsync(uint id) =>
            _roomRepository.GetRoomByIdAsync(id);

        public Task RemoveFromRoom(ISession session) =>
            _roomRepository.RemoveFromRoom(session);

        public ICollection<IRoom> GetAllRooms() =>
            _roomRepository.GetAllRooms();

        public async Task<ICollection<IRoomData>> GetAllRoomDataById(uint playerId) =>
            await _roomRepository.GetAllRoomDataById(playerId);

        public bool TryGetRoomModel(string modelName, out IRoomModel model) =>
            _roomRepository.TryGetRoomModel(modelName, out model);

        public bool TryGetRoom(uint roomId, out IRoom room) =>
            _roomRepository.TryGetRoom(roomId, out room);
    }

    public interface IRoomController
    {
        Task<int> AddNewRoomAsync(IRoomData roomData);
        Task GiveRoomRights(uint roomId, uint playerId);
        Task TakeRoomRights(uint roomId, uint playerId);
        Task<IDictionary<uint, string>> GetRightsForRoomAsync(uint roomId);
        Task<IRoom> GetRoomByIdAsync(uint id);
        Task<IRoom> GetRoomByIdAndPassword(uint id, string password);
        Task RemoveFromRoom(ISession session);
        ICollection<IRoom> GetAllRooms();
        Task<ICollection<IRoomData>> GetAllRoomDataById(uint playerId);
        bool TryGetRoomModel(string modelName, out IRoomModel model);
        bool TryGetRoom(uint roomId, out IRoom room);
    }
}
