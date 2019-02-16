using System.Threading.Tasks;

namespace AliasPro.Room
{
    using Models;
    using Sessions;
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
        Task<IRoom> GetRoomByIdAsync(uint id);
        Task<IRoom> GetRoomByIdAndPassword(uint id, string password);
        Task RemoveFromRoom(ISession session);
        ICollection<IRoom> GetAllRooms();
        Task<ICollection<IRoomData>> GetAllRoomDataById(uint playerId);
        bool TryGetRoomModel(string modelName, out IRoomModel model);
        bool TryGetRoom(uint roomId, out IRoom room);
    }
}
