using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Player
{
    using Models;
    using Models.Currency;
    using Models.Messenger;
    using Models.Badge;

    internal class PlayerController : IPlayerController
    {
        private readonly PlayerRepostiory _playerRepostiory;

        public PlayerController(PlayerRepostiory playerRepostiory)
        {
            _playerRepostiory = playerRepostiory;
        }
        
        public async Task UpdateStatus(IPlayer player, ICollection<IMessengerFriend> friends) =>
            await _playerRepostiory.UpdateStatus(player, friends);

        public async Task UpdatePlayerByIdAsync(IPlayer player) =>
            await _playerRepostiory.UpdatePlayerById(player);

        public async Task AddPlayerSettingsAsync(uint id) =>
            await _playerRepostiory.CreatePlayerSettings(id);

        public async Task AddFriendRequestAsync(uint playerId, uint targetId) =>
            await _playerRepostiory.CreateFriendRequest(playerId, targetId);

        public async Task AddFriendShipAsync(uint playerId, uint targetId) =>
            await _playerRepostiory.CreateFriendShip(playerId, targetId);

        public async Task AddOfflineMessageAsync(uint playerId, IMessengerMessage privateMessage) =>
            await _playerRepostiory.CreateOfflineMessage(playerId, privateMessage);

        public async Task<IPlayer> GetPlayerByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerById(id);

        public async Task<IPlayer> GetPlayerBySsoAsync(string sso) =>
            await _playerRepostiory.GetPlayerBySso(sso);

        public async Task<IPlayer> GetPlayerByUsernameAsync(string username) =>
            await _playerRepostiory.GetPlayerByUsername(username);
        
        public async Task<IPlayerSettings> GetPlayerSettingsByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerSettingsById(id);

        public async Task<IDictionary<string, IBadgeData>> GetPlayerBadgesByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerBadgesById(id);
        
        public async Task<IDictionary<int, ICurrencyType>> GetPlayerCurrenciesByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerCurrenciesById(id);

        public async Task<IDictionary<uint, IMessengerFriend>> GetPlayerFriendsByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerFriendsById(id);

        public async Task<IDictionary<uint, IMessengerRequest>> GetPlayerRequestsByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerRequestsById(id);

        public async Task<IDictionary<uint, IPlayer>> GetPlayersByUsernameAsync(string username) =>
            await _playerRepostiory.GetPlayersByUsername(username);

        public async Task<ICollection<IMessengerMessage>> GetOfflineMessagesAsync(uint playerId) =>
            await _playerRepostiory.GetOfflineMessages(playerId);
        
        public async Task RemoveAllFriendRequestsAsync(uint playerId) =>
            await _playerRepostiory.RemoveAllFriendRequests(playerId);

        public async Task RemoveFriendRequestAsync(uint playerId, uint targetId) =>
            await _playerRepostiory.RemoveFriendRequest(playerId, targetId);

        public async Task RemoveFriendShipAsync(uint playerId, uint targetId) =>
            await _playerRepostiory.RemoveFriendShip(playerId, targetId);

        public async Task RemovePlayerByIdAsync(uint playerId) =>
            await _playerRepostiory.RemovePlayerById(playerId);

        public async Task ResetPlayerWearableBadgesAsync(uint playerId) =>
           await _playerRepostiory.ResetPlayerWearableBadges(playerId);

        public async Task UpdatePlayerWearableBadgeAsync(uint playerId, string code, int slot) =>
           await _playerRepostiory.UpdatePlayerWearableBadge(playerId, code, slot);
    }

    public interface IPlayerController
    {
        Task UpdateStatus(IPlayer player, ICollection<IMessengerFriend> friends);
        Task UpdatePlayerByIdAsync(IPlayer player);
        Task AddPlayerSettingsAsync(uint id);
        Task AddFriendRequestAsync(uint playerId, uint targetId);
        Task AddFriendShipAsync(uint playerId, uint targetId);
        Task AddOfflineMessageAsync(uint playerId, IMessengerMessage privateMessage);
        Task<IPlayer> GetPlayerByIdAsync(uint id);
        Task<IPlayer> GetPlayerBySsoAsync(string sso);
        Task<IPlayer> GetPlayerByUsernameAsync(string username);
        Task<IPlayerSettings> GetPlayerSettingsByIdAsync(uint id);
        Task<IDictionary<int, ICurrencyType>> GetPlayerCurrenciesByIdAsync(uint id);
        Task<IDictionary<string, IBadgeData>> GetPlayerBadgesByIdAsync(uint id);
        Task<IDictionary<uint, IMessengerFriend>> GetPlayerFriendsByIdAsync(uint id);
        Task<IDictionary<uint, IMessengerRequest>> GetPlayerRequestsByIdAsync(uint id);
        Task<IDictionary<uint, IPlayer>> GetPlayersByUsernameAsync(string username);
        Task<ICollection<IMessengerMessage>> GetOfflineMessagesAsync(uint playerId);
        Task RemoveAllFriendRequestsAsync(uint playerId);
        Task RemoveFriendRequestAsync(uint playerId, uint targetId);
        Task RemoveFriendShipAsync(uint playerId, uint targetId);
        Task RemovePlayerByIdAsync(uint playerId);
        Task ResetPlayerWearableBadgesAsync(uint playerId);
        Task UpdatePlayerWearableBadgeAsync(uint playerId, string code, int slot);
    }
}
