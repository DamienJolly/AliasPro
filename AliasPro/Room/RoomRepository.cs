using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Room
{
    using Models;
    using System.Linq;

    internal class RoomRepository
    {
        private readonly RoomDao _roomDao;

        private readonly IDictionary<int, IRoom> _rooms;
        private readonly IDictionary<string, IRoomModel> _roomModels;

        public RoomRepository(RoomDao roomDao)
        {
            _roomDao = roomDao;

            _rooms = new Dictionary<int, IRoom>();
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

        internal async Task<IRoom> GetRoomByIdAsync(int id)
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
                    _rooms.Add(id, room);
                    return room;
                }
            }

            return null;
        }

        internal IList<IRoom> GetRoomsByCategorySearch(uint categoryId, string searchCode) =>
            _rooms.Values.Where(x => x.RoomData.Name.Contains(searchCode)).ToList();
    }
}
