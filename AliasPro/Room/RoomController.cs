using System.Threading.Tasks;

namespace AliasPro.Room
{
    using Models;
    using Models.Entities;
    using Sessions;
    using System.Collections.Generic;

    internal class RoomController : IRoomController
    {
        private readonly RoomRepository _roomRepository;

        public RoomController(RoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<BaseEntity> AddUserToRoom(IRoom room, ISession session)
        {
            IRoomModel roomModel = room.RoomModel;
            UserEntity userEntity = new UserEntity(room.Entities.Count + 1, roomModel.DoorX, roomModel.DoorY, roomModel.DoorDir, session);
            session.Entity = userEntity;
            await room.AddEntity(userEntity);

            return userEntity;
        }

        public Task<IRoom> GetRoomByIdAsync(int id) =>
            _roomRepository.GetRoomByIdAsync(id);

        public async Task<IRoom> GetRoomByIdAndPassword(int id, string password)
        {
            IRoom room = await GetRoomByIdAsync(id);
            if (room != null)
            {
                if (room.RoomData.Password == password)
                {
                    return room;
                }
            }

            return null;
        }

        public ICollection<IRoom> GetAllRooms() =>
            _roomRepository.GetAllRooms();
    }

    public interface IRoomController
    {
        Task<BaseEntity> AddUserToRoom(IRoom room, ISession session);
        Task<IRoom> GetRoomByIdAsync(int id);
        Task<IRoom> GetRoomByIdAndPassword(int id, string password);
        ICollection<IRoom> GetAllRooms();
    }
}
