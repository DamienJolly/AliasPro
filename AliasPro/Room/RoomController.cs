﻿using System.Threading.Tasks;

namespace AliasPro.Room
{
    using Models;

    internal class RoomController : IRoomController
    {
        private readonly RoomRepository _roomRepository;

        public RoomController(RoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
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
    }
}
