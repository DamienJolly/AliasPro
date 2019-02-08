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

        public bool TryGetRoomModel(string modelName, out IRoomModel model) =>
            _roomRepository.TryGetRoomModel(modelName, out model);
    }

    public interface IRoomController
    {
        Task<int> AddNewRoomAsync(IRoomData roomData);
        Task<IRoom> GetRoomByIdAsync(uint id);
        Task<IRoom> GetRoomByIdAndPassword(uint id, string password);
        Task RemoveFromRoom(ISession session);
        ICollection<IRoom> GetAllRooms();
        bool TryGetRoomModel(string modelName, out IRoomModel model);
    }
}
