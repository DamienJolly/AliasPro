using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Room
{
    using Models;
    using Sessions;

    internal class RoomRepository
    {
        private readonly RoomDao _roomDao;

        private readonly IDictionary<uint, IRoom> _rooms;
        private readonly IDictionary<string, IRoomModel> _roomModels;

        public RoomRepository(RoomDao roomDao)
        {
            _roomDao = roomDao;

            _rooms = new Dictionary<uint, IRoom>();
            _roomModels = new Dictionary<string, IRoomModel>();
            LoadRoomModels();
        }

        private async void LoadRoomModels()
        {
            IEnumerable<IRoomModel> roomModels = await _roomDao.GetRoomModels();
            foreach (IRoomModel roomModel in roomModels)
            {
                _roomModels.Add(roomModel.Id, roomModel);
            }
        }

        internal async Task<IRoom> GetRoomByIdAsync(uint id)
        {
            if (_rooms.TryGetValue(id, out IRoom room))
            {
                return room;
            }
            
            IRoomData roomData = await _roomDao.GetRoomData(id);
            if (roomData != null)
            {
                if (_roomModels.TryGetValue(roomData.ModelName, out IRoomModel model))
                {
                    room = new Room(roomData, model);
                    _rooms.Add(roomData.Id, room);
                    return room;
                }
            }

            return null;
        }

        internal async Task<IRoom> GetRoomByIdAndPassword(uint id, string password)
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

        internal async Task RemoveFromRoom(ISession session)
        {
            IRoom room = session.CurrentRoom;
            if (session.Entity != null)
            {
                await room.RemoveEntity(session.Entity.Id);
            }
            if (!room.EntityHandler.HasUserEntities)
            {
                await _roomDao.UpdateRoomItems(room.ItemHandler.Items);
            }
            _rooms.Remove(room.RoomData.Id);
            room = null;
        }

        internal ICollection<IRoom> GetAllRooms() =>
            _rooms.Values;

        internal bool TryGetRoomModel(string modelName, out IRoomModel model) =>
            _roomModels.TryGetValue(modelName, out model);
    }
}
