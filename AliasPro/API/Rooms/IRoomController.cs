using AliasPro.API.Rooms.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Rooms
{
    public interface IRoomController
    {
        void Cycle();
        ICollection<IRoom> Rooms { get; }

		Task<ICollection<IRoomData>> GetPlayersRoomsAsync(uint playerId);
        bool TryGetRoom(uint roomId, out IRoom room);
		Task<IRoom> LoadRoom(uint roomId);
		Task<int> CreateRoomAsync(uint playerId, string name, string description, string modelName, int categoryId, int maxUsers, int tradeType);

        bool TryGetRoomModel(string modelName, out IRoomModel model);

        Task<IRoomSettings> GetRoomSettingsAsync(uint roomId);

        Task<IDictionary<uint, string>> GetRightsForRoomAsync(uint roomId);
        Task GiveRoomRights(uint roomId, uint playerId);
        Task TakeRoomRights(uint roomId, uint playerId);
    }
}
