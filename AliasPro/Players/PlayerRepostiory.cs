﻿using AliasPro.API.Players.Models;
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
        
        public async Task<IPlayerData> GetPlayerDataAsync(string SSO) =>
            await _playerDao.GetPlayerDataAsync(SSO);

        public async Task<IPlayerData> GetPlayerDataAsync(uint playerId) =>
           await _playerDao.GetPlayerDataAsync(playerId);

        public async Task<IPlayerData> GetPlayerDataByUsernameAsync(string username) =>
           await _playerDao.GetPlayerDataByUsernameAsync(username);

        public async Task<int> GetPlayerFriendsAsync(uint playerId) =>
            await _playerDao.GetPlayerFriendsAsync(playerId);

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

		public async Task<uint> TryGetPlayerIdByUsername(string username) =>
			await _playerDao.GetPlayerIdByUsername(username);

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

        internal async Task<IDictionary<int, string>> GetPlayerIgnoresAsync(uint id) =>
            await _playerDao.GetPlayerIgnoresAsync(id);

        internal async Task AddPlayerIgnoreAsync(int playerId, int targetId) =>
            await _playerDao.AddPlayerIgnoreAsync(playerId, targetId);

        internal async Task RemovePlayerIgnoreAsync(int playerId, int targetId) =>
            await _playerDao.RemovePlayerIgnoreAsync(playerId, targetId);

        internal async Task<IList<int>> GetPlayerRecipesAsync(uint playerId) =>
            await _playerDao.GetPlayerRecipesAsync(playerId);

        internal async Task AddPlayerRecipeAsync(int playerId, int recipeId) =>
            await _playerDao.AddPlayerRecipeAsync(playerId, recipeId);

        public async Task AddPlayerCurrencyAsync(int playerId, IPlayerCurrency currency) =>
            await _playerDao.AddPlayerCurrencyAsync(playerId, currency);

        public async Task UpdatePlayerCurrenciesAsync(IPlayer player) =>
            await _playerDao.UpdatePlayerCurrenciesAsync(player);


        internal async Task<IDictionary<string, IPlayerBadge>> GetPlayerBadgesAsync(uint id) =>
            await _playerDao.GetPlayerBadgesAsync(id);

		public async Task<IDictionary<int, IPlayerBot>> GetPlayerBotsAsync(uint id) =>
			await _playerDao.GetPlayerBotsAsync(id);

		public async Task<IDictionary<int, IPlayerPet>> GetPlayerPetsAsync(uint id) =>
			await _playerDao.GetPlayerPetsAsync(id);

		internal async Task<IDictionary<int, IPlayerAchievement>> GetPlayerAchievementsAsync(uint id) =>
			await _playerDao.GetPlayerAchievementsAsync(id);

        internal async Task<IList<IPlayerSanction>> GetPlayerSanctionsAsync(uint id) =>
            await _playerDao.GetPlayerSanctionsAsync(id);

        internal async Task AddPlayerSanction(uint playerId, IPlayerSanction sanction) =>
            await _playerDao.AddPlayerSanction(playerId, sanction);

        public async Task UpdatePlayerBadgesAsync(IPlayer player) =>
            await _playerDao.UpdatePlayerBadgesAsync(player);


        public async Task<ICollection<IPlayerRoomVisited>> GetPlayerRoomVisitsAsync(uint playerId) =>
            await _playerDao.GetPlayerRoomVisitsAsync(playerId);

		public async Task<IDictionary<uint, IPlayerData>> GetPlayersByUsernameAsync(string playerName) =>
			 await _playerDao.GetPlayersByUsernameAsync(playerName);

        public async Task RemoveFavoriteGroup(int playerId, int groupId) =>
            await _playerDao.RemoveFavoriteGroup(playerId, groupId);

        public async Task AddPlayerBadge(uint playerId, IPlayerBadge badge) =>
           await _playerDao.AddPlayerBadge(playerId, badge);

        public async Task UpdatePlayerBadge(uint playerId, string oldCode, string newCode) =>
           await _playerDao.UpdatePlayerBadge(playerId, oldCode, newCode);

        public async Task RemovePlayerBadge(uint playerId, string code) =>
           await _playerDao.RemovePlayerBadge(playerId, code);

        public async Task AddPlayerAchievementAsync(int id, int progress, uint playerId) =>
            await _playerDao.AddPlayerAchievementAsync(id, progress, playerId);

        public async Task UpdatePlayerAchievementAsync(int id, int progress, uint playerId) =>
            await _playerDao.UpdatePlayerAchievementAsync(id, progress, playerId);
    }
}
