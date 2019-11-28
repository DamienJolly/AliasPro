using AliasPro.API.Players.Models;
using AliasPro.Items;
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

        public async Task<IDictionary<int, ICurrencySetting>> GetCurrencySettings() =>
            await _playerDao.GetCurrencySettings();
        
        public async Task<IPlayerData> GetPlayerDataAsync(string SSO) =>
            await _playerDao.GetPlayerDataAsync(SSO);

        public async Task<IPlayerData> GetPlayerDataAsync(uint playerId) =>
           await _playerDao.GetPlayerDataAsync(playerId);

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

		public async Task<IDictionary<int, IPlayerBot>> GetPlayerBotsAsync(uint id) =>
			await _playerDao.GetPlayerBotsAsync(id);

		internal async Task<IDictionary<int, IPlayerAchievement>> GetPlayerAchievementsAsync(uint id) =>
			await _playerDao.GetPlayerAchievementsAsync(id);

		public async Task UpdatePlayerBadgesAsync(IPlayer player) =>
            await _playerDao.UpdatePlayerBadgesAsync(player);


        public async Task<ICollection<IPlayerRoomVisited>> GetPlayerRoomVisitsAsync(uint playerId) =>
            await _playerDao.GetPlayerRoomVisitsAsync(playerId);

		public async Task<IDictionary<uint, IPlayerData>> GetPlayersByUsernameAsync(string playerName) =>
			 await _playerDao.GetPlayersByUsernameAsync(playerName);
	}
}
