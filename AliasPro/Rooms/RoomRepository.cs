using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Rooms
{
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

        public ICollection<IRoom> Rooms =>
            _rooms.Values.OrderBy(x => x.UsersNow).ToList();

        public async Task<ICollection<IRoomData>> GetPlayersRoomsAsync(uint playerId) =>
            await _roomDao.GetAllRoomDataById(playerId);

        public async Task<IRoomData> GetRoomDataAsync(uint roomId) =>
            await _roomDao.GetRoomData(roomId);

        public bool TryAddRoom(IRoom room) =>
            _rooms.TryAdd(room.Id, room);

        public void RemoveRoom(IRoom room) =>
            _rooms.Remove(room.Id);

        public bool TryGetRoom(uint roomId, out IRoom room) =>
            _rooms.TryGetValue(roomId, out room);

        public async Task<int> CreateRoomAsync(uint playerId, string name, string description, string modelName, int categoryId, int maxUsers, int tradeType) =>
            await _roomDao.CreateRoomAsync(playerId, name, description, modelName, categoryId, maxUsers, tradeType);


        public bool TryGetRoomModel(string modelName, out IRoomModel model) =>
            _roomModels.TryGetValue(modelName, out model);


        public async Task<IRoomSettings> GetRoomSettingsAsync(uint roomId) =>
            await _roomDao.GetRoomSettingsId(roomId);

        public async Task CreateRoomSettingsAsync(uint roomId) =>
            await _roomDao.CreateRoomSettings(roomId);


        public async Task<IDictionary<uint, string>> GetRightsForRoomAsync(uint roomId) =>
            await _roomDao.GetRightsForRoom(roomId);

        internal async Task CreateRoomRights(uint roomId, uint playerId) =>
            await _roomDao.CreateRoomRights(roomId, playerId);

        internal async Task RemoveRoomRights(uint roomId, uint playerId) =>
            await _roomDao.RemoveRoomRights(roomId, playerId);
    }
}
