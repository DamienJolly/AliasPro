using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Rooms
{
	internal class RoomController : IRoomController
	{
		private readonly RoomDao _roomDao;

		private readonly IDictionary<uint, IRoom> _rooms;
		private readonly IDictionary<string, IRoomModel> _roomModels;

		public RoomController(RoomDao roomDao)
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

		public void Cycle()
		{
			foreach (IRoom room in Rooms.ToList())
			{
				if (room.Loaded)
					room.RoomCycle.Cycle();

				if (!room.Loaded || !room.Entities.HasUserEntities)
					room.IdleTimer++;

				if (room.IdleTimer >= 60)
				{
					// todo: save room.
					room.Dispose();
					_rooms.Remove(room.Id);
				}
			}
		}

		public ICollection<IRoom> Rooms =>
			_rooms.Values;

		public async void LoadPlayersRooms(uint playerId)
		{
			ICollection<IRoomData> roomDatas = await _roomDao.GetAllRoomDataById(playerId);
			foreach (IRoomData roomData in roomDatas)
			{
				IRoom room = new Room(roomData);
				_rooms.TryAdd(room.Id, room);
			}
		}

		public bool TryGetRoom(uint roomId, out IRoom room) =>
			_rooms.TryGetValue(roomId, out room);

		public async Task<IRoom> LoadRoom(uint roomId)
		{
			if (!TryGetRoom(roomId, out IRoom room))
			{
				IRoomData roomData = await _roomDao.GetRoomData(roomId);
				if (roomData == null)
					return null;

				room = new Room(roomData);
				_rooms.TryAdd(room.Id, room);
			}

			return room;
		}

        public async Task<int> CreateRoomAsync(uint playerId, string name, string description, string modelName, int categoryId, int maxUsers, int tradeType) =>
            await _roomDao.CreateRoomAsync(playerId, name, description, modelName, categoryId, maxUsers, tradeType);


        public bool TryGetRoomModel(string modelName, out IRoomModel model) =>
			_roomModels.TryGetValue(modelName, out model);


		public async Task<IRoomSettings> GetRoomSettingsAsync(uint roomId)
		{
			IRoomSettings roomSettings =
						await _roomDao.GetRoomSettingsId(roomId);

			if (roomSettings == null)
			{
				await _roomDao.CreateRoomSettings(roomId);
				roomSettings =
					await _roomDao.GetRoomSettingsId(roomId);
			}
			return roomSettings;
		}

		public async Task<IDictionary<uint, string>> GetRightsForRoomAsync(uint roomId) =>
			await _roomDao.GetRightsForRoom(roomId);

		public async Task GiveRoomRights(uint roomId, uint playerId) =>
			await _roomDao.CreateRoomRights(roomId, playerId);

		public async Task TakeRoomRights(uint roomId, uint playerId) =>
			await _roomDao.RemoveRoomRights(roomId, playerId);
	}
}
