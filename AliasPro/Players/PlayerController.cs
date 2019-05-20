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

        public PlayerController(PlayerRepostiory playerRepostiory)
        {
            _playerRepostiory = playerRepostiory;
        }

        public void Cycle()
        {
            foreach (IPlayer player in Players.ToList())
            {
                if (player.PlayerCycle != null)
                    player.PlayerCycle.Cycle();
            }
        }

        public ICollection<IPlayer> Players =>
            _playerRepostiory.Players;

        public async Task<IPlayerData> GetPlayerDataAsync(string SSO) =>
            await _playerRepostiory.GetPlayerDataAsync(SSO);

        public async Task<IPlayerData> GetPlayerDataAsync(uint playerId) =>
            await _playerRepostiory.GetPlayerDataAsync(playerId);

        public bool TryGetPlayer(uint playerId, out IPlayer player) =>
            _playerRepostiory.TryGetPlayer(playerId, out player);

        public bool TryGetPlayer(string playerUsername, out IPlayer player) =>
            _playerRepostiory.TryGetPlayer(playerUsername, out player);

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

        public async Task UpdatePlayerCurrenciesAsync(IPlayer player) =>
            await _playerRepostiory.UpdatePlayerCurrenciesAsync(player);


        public async Task<IDictionary<string, IPlayerBadge>> GetPlayerBadgesAsync(uint id) =>
            await _playerRepostiory.GetPlayerBadgesAsync(id);

        public async Task UpdatePlayerBadgesAsync(IPlayer player) =>
            await _playerRepostiory.UpdatePlayerBadgesAsync(player);


        public async Task<ICollection<IPlayerRoomVisited>> GetPlayerRoomVisitsAsync(uint playerId) =>
            await _playerRepostiory.GetPlayerRoomVisitsAsync(playerId);
    }
}
