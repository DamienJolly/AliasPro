﻿using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Players
{
    internal class PlayerController : IPlayerController
    {
        private readonly PlayerRepostiory _playerRepostiory;
        private IDictionary<int, ICurrencySetting> _currencySettings;

        public PlayerController(PlayerRepostiory playerRepostiory)
        {
            _playerRepostiory = playerRepostiory;
            _currencySettings = new Dictionary<int, ICurrencySetting>();

            LoadCurrencySettings();
        }

        public async void LoadCurrencySettings()
        {
            if (_currencySettings.Count > 0) _currencySettings.Clear();

            _currencySettings =
                await _playerRepostiory.GetCurrencySettings();
        }

        public void Cycle()
        {
            foreach (IPlayer player in Players.ToList())
            {
                if (player.PlayerCycle != null)
                    player.PlayerCycle.Cycle(_currencySettings);
            }
        }

        public ICollection<IPlayer> Players =>
            _playerRepostiory.Players;

        public async Task<IPlayerData> GetPlayerDataAsync(string SSO) =>
            await _playerRepostiory.GetPlayerDataAsync(SSO);

        public async Task<IPlayerData> GetPlayerDataAsync(uint playerId)
        {
            if (TryGetPlayer(playerId, out IPlayer player))
                return player;

            return await _playerRepostiory.GetPlayerDataAsync(playerId);
        }

        public async Task<int> GetPlayerFriendsAsync(uint playerId) =>
            await _playerRepostiory.GetPlayerFriendsAsync(playerId);

        public bool TryGetPlayer(uint playerId, out IPlayer player) =>
            _playerRepostiory.TryGetPlayer(playerId, out player);

        public bool TryGetPlayer(string playerUsername, out IPlayer player) =>
            _playerRepostiory.TryGetPlayer(playerUsername, out player);

		public async Task<uint> TryGetPlayerIdByUsername(string username) =>
			await _playerRepostiory.TryGetPlayerIdByUsername(username);

		public bool TryAddPlayer(IPlayer player) =>
            _playerRepostiory.TryAddPlayer(player);

        public void RemovePlayer(IPlayer player) =>
            _playerRepostiory.RemovePlayer(player);

        public async Task UpdatePlayerAsync(IPlayer player) =>
            await _playerRepostiory.UpdatePlayerAsync(player);


        public async Task<IPlayerSettings> GetPlayerSettingsAsync(uint id) =>
            await _playerRepostiory.GetPlayerSettingsAsync(id);

        public async Task AddPlayerSettingsAsync(uint id) =>
            await _playerRepostiory.AddPlayerSettingsAsync(id);

        public async Task UpdatePlayerSettingsAsync(IPlayer player) =>
            await _playerRepostiory.UpdatePlayerSettingsAsync(player);
        

        public async Task<IDictionary<int, IPlayerCurrency>> GetPlayerCurrenciesAsync(uint id) =>
            await _playerRepostiory.GetPlayerCurrenciesAsync(id);

        public async Task<IDictionary<int, string>> GetPlayerIgnoresAsync(uint id) =>
            await _playerRepostiory.GetPlayerIgnoresAsync(id);

        public async Task AddPlayerIgnoreAsync(int playerId, int targetId) =>
            await _playerRepostiory.AddPlayerIgnoreAsync(playerId, targetId);

        public async Task RemovePlayerIgnoreAsync(int playerId, int targetId) =>
            await _playerRepostiory.RemovePlayerIgnoreAsync(playerId, targetId);

        public async Task UpdatePlayerCurrenciesAsync(IPlayer player) =>
            await _playerRepostiory.UpdatePlayerCurrenciesAsync(player);


        public async Task<IDictionary<string, IPlayerBadge>> GetPlayerBadgesAsync(uint id) =>
            await _playerRepostiory.GetPlayerBadgesAsync(id);

		public async Task<IDictionary<int, IPlayerBot>> GetPlayerBotsAsync(uint id) =>
			await _playerRepostiory.GetPlayerBotsAsync(id);

		public async Task<IDictionary<int, IPlayerPet>> GetPlayerPetsAsync(uint id) =>
			await _playerRepostiory.GetPlayerPetsAsync(id);

		public async Task<IDictionary<int, IPlayerAchievement>> GetPlayerAchievementsAsync(uint id) =>
			await _playerRepostiory.GetPlayerAchievementsAsync(id);

		public async Task UpdatePlayerBadgesAsync(IPlayer player) =>
            await _playerRepostiory.UpdatePlayerBadgesAsync(player);


        public async Task<ICollection<IPlayerRoomVisited>> GetPlayerRoomVisitsAsync(uint playerId) =>
            await _playerRepostiory.GetPlayerRoomVisitsAsync(playerId);

		public async Task<IDictionary<uint, IPlayerData>> GetPlayersByUsernameAsync(string playerName) =>
			 await _playerRepostiory.GetPlayersByUsernameAsync(playerName);

        public async Task RemoveFavoriteGroup(int playerId, int groupId) =>
            await _playerRepostiory.RemoveFavoriteGroup(playerId, groupId);
    }
}
