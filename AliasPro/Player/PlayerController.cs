using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Player
{
    using Models;
    using Models.Currency;
    using Models.Messenger;

    internal class PlayerController : IPlayerController
    {
        private readonly PlayerRepostiory _playerRepostiory;

        public PlayerController(PlayerRepostiory playerRepostiory)
        {
            _playerRepostiory = playerRepostiory;
        }

        public async Task AddPlayerSettingsAsync(uint id) =>
            await _playerRepostiory.CreatePlayerSettings(id);

        public async Task AddFriendRequestAsync(uint playerId, uint targetId) =>
            await _playerRepostiory.CreateFriendRequest(playerId, targetId);

        public async Task AddFriendShipAsync(uint playerId, uint targetId) =>
            await _playerRepostiory.CreateFriendShip(playerId, targetId);
        
        public async Task<IPlayer> GetPlayerByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerById(id);

        public async Task<IPlayer> GetPlayerBySsoAsync(string sso) =>
            await _playerRepostiory.GetPlayerBySso(sso);

        public async Task<IPlayer> GetPlayerByUsernameAsync(string username) =>
            await _playerRepostiory.GetPlayerByUsername(username);
        
        public async Task<IPlayerSettings> GetPlayerSettingsByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerSettingsById(id);

        public async Task<IDictionary<int, ICurrencyType>> GetPlayerCurrenciesByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerCurrenciesById(id);

        public async Task<IDictionary<uint, IMessengerFriend>> GetPlayerFriendsByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerFriendsById(id);

        public async Task<IDictionary<uint, IMessengerRequest>> GetPlayerRequestsByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerRequestsById(id);

        public async Task<IDictionary<uint, IPlayer>> GetPlayersByUsernameAsync(string username) =>
            await _playerRepostiory.GetPlayersByUsername(username);

        public async Task UpdatePlayerSettingsAsync(uint id, IPlayerSettings settings) =>
            await _playerRepostiory.UpdatePlayerSettings(id, settings);

        public async Task RemoveAllFriendRequestsAsync(uint playerId) =>
            await _playerRepostiory.RemoveAllFriendRequests(playerId);

        public async Task RemoveFriendRequestAsync(uint playerId, uint targetId) =>
            await _playerRepostiory.RemoveFriendRequest(playerId, targetId);

        public async Task RemoveFriendShipAsync(uint playerId, uint targetId) =>
            await _playerRepostiory.RemoveFriendShip(playerId, targetId);
    }

    public interface IPlayerController
    {
        Task AddPlayerSettingsAsync(uint id);
        Task AddFriendRequestAsync(uint playerId, uint targetId);
        Task AddFriendShipAsync(uint playerId, uint targetId);
        Task<IPlayer> GetPlayerByIdAsync(uint id);
        Task<IPlayer> GetPlayerBySsoAsync(string sso);
        Task<IPlayer> GetPlayerByUsernameAsync(string username);
        Task<IPlayerSettings> GetPlayerSettingsByIdAsync(uint id);
        Task<IDictionary<int, ICurrencyType>> GetPlayerCurrenciesByIdAsync(uint id);
        Task<IDictionary<uint, IMessengerFriend>> GetPlayerFriendsByIdAsync(uint id);
        Task<IDictionary<uint, IMessengerRequest>> GetPlayerRequestsByIdAsync(uint id);
        Task<IDictionary<uint, IPlayer>> GetPlayersByUsernameAsync(string username);
        Task UpdatePlayerSettingsAsync(uint id, IPlayerSettings settings);
        Task RemoveAllFriendRequestsAsync(uint playerId);
        Task RemoveFriendRequestAsync(uint playerId, uint targetId);
        Task RemoveFriendShipAsync(uint playerId, uint targetId);

    }
}
