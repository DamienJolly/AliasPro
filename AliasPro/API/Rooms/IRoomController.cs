using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Rooms
{
    public interface IRoomController
    {
        void Cycle();
        ICollection<IRoom> Rooms { get; }

		void LoadPlayersRooms(uint playerId);
		bool TryGetRoom(uint roomId, out IRoom room);
		Task<IRoom> LoadRoom(uint roomId);
		Task<int> CreateRoomAsync(uint playerId, string name, string description, string modelName, int categoryId, int maxUsers, int tradeType);

        bool TryGetRoomModel(string modelName, out IRoomModel model);

        Task<IRoomSettings> GetRoomSettingsAsync(uint roomId);

		Task<IDictionary<int, BaseEntity>> GetBotsForRoomAsync(IRoom room);
		Task<IDictionary<int, BaseEntity>> GetPetsForRoomAsync(IRoom room);
		Task UpdateBotSettings(BaseEntity entity, uint roomId);
		Task UpdatePetSettings(BaseEntity entity, uint roomId);

		Task<IDictionary<uint, string>> GetRightsForRoomAsync(uint roomId);
        Task GiveRoomRights(uint roomId, uint playerId);
        Task TakeRoomRights(uint roomId, uint playerId);
		void DisposeRoom(IRoom room);
		Task UpdateRoomModel(IRoomModel model);
		Task<bool> TryAddRoomModel(IRoomModel model);
		Task<IList<string>> GetWordFilterForRoomAsync(uint roomId);
		Task AddRoomWordFilter(string word, IRoom room);
		Task RemoveRoomWordFilter(string word, IRoom room);
	}
}
