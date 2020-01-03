using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Entities;
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
					DisposeRoom(room);
			}
		}

		public async void DisposeRoom(IRoom room)
		{
			if (room.Entities != null)
			{
				foreach (BaseEntity entity in room.Entities.BotEntities)
					await UpdateBotSettings(entity, room.Id);

				foreach (BaseEntity entity in room.Entities.PetEntities)
					await UpdatePetSettings(entity, room.Id);

				foreach (BaseEntity entity in room.Entities.PlayerEntities)
					await room.RemoveEntity(entity, true);
			}

			await UpdateRoom(room);

			room.Dispose();
			_rooms.Remove(room.Id);
		}

		public ICollection<IRoom> Rooms =>
			_rooms.Values;

		public async Task<ICollection<IRoomData>> GetPlayersRooms(uint playerId)
		{
			IList<IRoomData> roomsToGo = new List<IRoomData>();
			foreach (IRoomData roomData in await _roomDao.GetAllRoomDataById(playerId))
			{
				if (TryGetRoom(roomData.Id, out IRoom room))
					roomsToGo.Add(room);
				else
					roomsToGo.Add(roomData);
			}
			return roomsToGo;
		}

		public async void LoadPlayersRooms(uint playerId)
		{
			ICollection<IRoomData> roomDatas = await _roomDao.GetAllRoomDataById(playerId);
			foreach (IRoomData roomData in roomDatas)
			{
				IRoom room = new Room(roomData);
				_rooms.TryAdd(room.Id, room);
			}
		}

		public IDictionary<int, IList<IRoomData>> GetRoomsByCategory()
		{
			IDictionary<int, IList<IRoomData>> rooms = new Dictionary<int, IList<IRoomData>>();
			foreach (IRoomData data in _rooms.Values)
			{
				if (!rooms.ContainsKey(data.CategoryId))
					rooms.Add(data.CategoryId, new List<IRoomData>());

				rooms[data.CategoryId].Add(data);
			}
			return rooms;
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

		public async Task<bool> TryAddRoomModel(IRoomModel model)
		{
			await _roomDao.CreateRoomModelAsync(model);
			return _roomModels.TryAdd(model.Id, model);
		}

		public async Task UpdateRoomModel(IRoomModel model) =>
			await _roomDao.UpdateRoomModelAsync(model);

		public bool TryGetRoomModel(string modelName, out IRoomModel model) =>
			_roomModels.TryGetValue(modelName, out model);

		public async Task<int> AddRoomPromotion(uint roomId, IRoomPromotion promotion) =>
			await _roomDao.CreateRoomPromotionAsync(roomId, promotion);

		public async Task UpdateRoomPromotion(IRoomPromotion promotion) =>
			await _roomDao.UpdateRoomPromotionAsync(promotion);

		public async Task UpdateRoom(IRoom room)
		{
			if (room.Settings != null)
				await _roomDao.UpdateRoomSettins(room);

			if (room.Items != null)
				await _roomDao.UpdateRoomItems(room.Items.Items);

			await _roomDao.UpdateRoom(room);
		}

		public async Task AddRoomWordFilter(string word, IRoom room)
		{
			if (room.WordFilter.Contains(word)) return;

			await _roomDao.CreateRoomWordFilterAsync(word, room.Id);
			room.WordFilter.Add(word);
		}

		public async Task RemoveRoomWordFilter(string word, IRoom room)
		{
			if (!room.WordFilter.Contains(word)) return;

			await _roomDao.RemoveRoomWordFilterAsync(word, room.Id);
			room.WordFilter.Remove(word);
		}

		public async Task<IDictionary<int, BaseEntity>> GetBotsForRoomAsync(IRoom room) =>
			await _roomDao.GetBotsForRoomAsync(room);

		public async Task<IDictionary<int, BaseEntity>> GetPetsForRoomAsync(IRoom room) =>
			await _roomDao.GetPetsForRoomAsync(room);

		public async Task UpdateBotSettings(BaseEntity entity, uint roomId) =>
			await _roomDao.UpdateBotSettings(entity, roomId);

		public async Task UpdatePetSettings(BaseEntity entity, uint roomId) =>
			await _roomDao.UpdatePetSettings(entity, roomId);

		public async Task<IDictionary<uint, string>> GetRightsForRoomAsync(uint roomId) =>
			await _roomDao.GetRightsForRoom(roomId);

		public async Task<IList<string>> GetWordFilterForRoomAsync(uint roomId) =>
			await _roomDao.GetWordFilterForRoomAsync(roomId);

		public async Task GiveRoomRights(uint roomId, uint playerId) =>
			await _roomDao.CreateRoomRights(roomId, playerId);

		public async Task TakeRoomRights(uint roomId, uint playerId) =>
			await _roomDao.RemoveRoomRights(roomId, playerId);
	}
}
