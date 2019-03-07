using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AliasPro.Room
{
    using Models;
    using Sessions;
    using System.Linq;

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

        internal async Task CreateRoomRights(uint roomId, uint playerId) =>
            await _roomDao.CreateRoomRights(roomId, playerId);

        internal async Task RemoveRoomRights(uint roomId, uint playerId) =>
            await _roomDao.RemoveRoomRights(roomId, playerId);
        
        internal async Task<IDictionary<uint, string>> GetRightsForRoom(uint roomId) =>
            await _roomDao.GetRightsForRoom(roomId);

        internal async Task<int> CreateRoom(IRoomData roomData) =>
            await _roomDao.CreateRoom(roomData);

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
                    roomData.Settings = 
                        await GetRoomSettingsIdAsync(room.RoomData.Id);
                    _rooms.Add(room.RoomData.Id, room);
                    return room;
                }
            }

            return null;
        }

        internal async Task<ICollection<IRoomData>> GetAllRoomDataById(uint playerId) =>
            await _roomDao.GetAllRoomDataById(playerId);

        internal async Task<IRoomSettings> GetRoomSettingsIdAsync(uint id)
        {
            IRoomSettings roomSettings =
                    await _roomDao.GetRoomSettingsId(id);

            if (roomSettings == null)
            {
                await _roomDao.CreateRoomSettings(id);
                roomSettings =
                    await _roomDao.GetRoomSettingsId(id);
            }

            return roomSettings;
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
            if (session.Entity != null)
            {
                await session.CurrentRoom.RemoveEntity(session.Entity);
                session.Entity = null;
            }
            if (!session.CurrentRoom.EntityHandler.HasUserEntities)
            {
                //todo: update settings
                await _roomDao.UpdateRoomItems(session.CurrentRoom.ItemHandler.Items);
                _rooms.Remove(session.CurrentRoom.RoomData.Id);
            }
            session.CurrentRoom = null;
        }

        internal ICollection<IRoom> GetAllRooms() =>
            _rooms.Values.OrderBy(x => x.RoomData.UsersNow).ToList();

        internal bool TryGetRoomModel(string modelName, out IRoomModel model) =>
            _roomModels.TryGetValue(modelName, out model);

        internal bool TryGetRoom(uint roomId, out IRoom room) =>
            _rooms.TryGetValue(roomId, out room);
    }
}
