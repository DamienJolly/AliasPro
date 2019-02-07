using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Player
{
    using Models;
    using Models.Currency;
    using Models.Messenger;

    internal class PlayerRepostiory
    {
        private readonly PlayerDao _playerDao;
        private readonly Dictionary<uint, IPlayer> _players;

        public PlayerRepostiory(PlayerDao playerDao)
        {
            _playerDao = playerDao;

            _players = new Dictionary<uint, IPlayer>();
        }

        internal async Task<IPlayer> GetPlayerById(uint id)
        {
            if (_players.TryGetValue(id, out IPlayer player)) return player;

            return await _playerDao.GetPlayerById(id);
        }

        internal async Task<IPlayer> GetPlayerByUsername(string username)
        {
            //todo: remove? idk
            foreach (IPlayer player in _players.Values)
            {
                if (player.Username == username)
                    return player;
            }

            return await _playerDao.GetPlayerByUsername(username);
        }

        internal async Task<IPlayer> GetPlayerBySso(string sso)
        {
            IPlayer player = await _playerDao.GetPlayerBySso(sso);
            if(!_players.ContainsKey(player.Id))
            {
                _players.Add(player.Id, player);
            }

            return player;
        }

        internal async Task CreatePlayerSettings(uint id) =>
            await _playerDao.CreatePlayerSettings(id);

        internal async Task CreateFriendRequest(uint playerId, uint targetId) =>
            await _playerDao.CreateFriendRequest(playerId, targetId);

        public async Task CreateFriendShip(uint playerId, uint targetId) =>
            await _playerDao.CreateFriendShip(playerId, targetId);

        public async Task CreateOfflineMessage(uint playerId, IMessengerMessage privateMessage) =>
            await _playerDao.CreateOfflineMessage(playerId, privateMessage);

        internal async Task<IPlayerSettings> GetPlayerSettingsById(uint id) =>
            await _playerDao.GetPlayerSettingsById(id);

        public async Task<IDictionary<int, ICurrencyType>> GetPlayerCurrenciesById(uint id) =>
            await _playerDao.GetPlayerCurrenciesById(id);

        public async Task<IDictionary<uint, IMessengerFriend>> GetPlayerFriendsById(uint id) =>
            await _playerDao.GetPlayerFriendsById(id);

        public async Task<IDictionary<uint, IMessengerRequest>> GetPlayerRequestsById(uint id) =>
            await _playerDao.GetPlayerRequestById(id);

        public async Task<IDictionary<uint, IPlayer>> GetPlayersByUsername(string username) =>
            await _playerDao.GetPlayersByUsername(username);

        public async Task<ICollection<IMessengerMessage>> GetOfflineMessages(uint playerId) =>
            await _playerDao.GetOfflineMessages(playerId);

        internal async Task UpdatePlayerSettings(uint id, IPlayerSettings settings) =>
            await _playerDao.UpdatePlayerSettings(id, settings);

        public async Task RemoveAllFriendRequests(uint playerId) =>
            await _playerDao.RemoveAllFriendRequests(playerId);

        public async Task RemoveFriendRequest(uint playerId, uint targetId) =>
            await _playerDao.RemoveFriendRequest(playerId, targetId);

        public async Task RemoveFriendShip(uint playerId, uint targetId) =>
            await _playerDao.RemoveFriendShip(playerId, targetId);
    }
}
