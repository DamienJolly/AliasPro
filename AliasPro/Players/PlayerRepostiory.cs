using AliasPro.API.Player.Models;
using AliasPro.Items;
using AliasPro.Players.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Players
{
    class PlayerRepostiory
    {
        private readonly PlayerDao _playerDao;
        private readonly ItemDao _itemDao;
        private readonly IDictionary<uint, IPlayer> _players;
        private readonly IDictionary<string, uint> _playerUsernames;

        public PlayerRepostiory(PlayerDao playerDao, ItemDao itemDao)
        {
            _playerDao = playerDao;
            _itemDao = itemDao;

            _players = new Dictionary<uint, IPlayer>();
            _playerUsernames = new Dictionary<string, uint>();
        }

        public ICollection<IPlayer> Players => _players.Values;
        
        public int Count => _players.Count;
        
        public async Task<IPlayerData> GetPlayerDataAsync(string SSO) =>
            await _playerDao.GetPlayerDataAsync(SSO);

        public bool TryAddPlayer(IPlayer player)
        {
            if (!_players.TryAdd(player.Id, player))
                return false;

            if (!_playerUsernames.TryAdd(player.Username, player.Id))
            {
                _players.Remove(player.Id);
                return false;
            }

            return true;
        }

        public void RemovePlayer(IPlayer player)
        {
            _players.Remove(player.Id);
            _playerUsernames.Remove(player.Username);
        }

        public bool TryGetPlayer(uint playerId, out IPlayer player) =>
            _players.TryGetValue(playerId, out player);

        public bool TryGetPlayer(string playerUsername, out IPlayer player)
        {
            if (_playerUsernames.TryGetValue(playerUsername, out uint playerId))
            {
                return _players.TryGetValue(playerId, out player);
            }
            else
            {
                player = null;
                return false;
            }
        }

        public async Task UpdatePlayerAsync(IPlayer player) =>
            await _playerDao.UpdatePlayerAsync(player);


        internal async Task<IPlayerSettings> GetPlayerSettingsAsync(uint id) =>
            await _playerDao.GetPlayerSettingsAsync(id);

        internal async Task AddPlayerSettingsAsync(uint id) =>
            await _playerDao.AddPlayerSettingsAsync(id);

        public async Task UpdatePlayerSettingsAsync(IPlayer player) =>
            await _playerDao.UpdatePlayerSettingsAsync(player);


        public async Task<IDictionary<int, IPlayerCurrency>> GetPlayerCurrenciesAsync(uint id) =>
            await _playerDao.GetPlayerCurrenciesAsync(id);

        public async Task UpdatePlayerCurrenciesAsync(IPlayer player) =>
            await _playerDao.UpdatePlayerCurrenciesAsync(player);


        internal async Task<IDictionary<string, IPlayerBadge>> GetPlayerBadgesAsync(uint id) =>
            await _playerDao.GetPlayerBadgesAsync(id);

        public async Task UpdatePlayerBadgesAsync(IPlayer player) =>
            await _playerDao.UpdatePlayerBadgesAsync(player);
    }
}
