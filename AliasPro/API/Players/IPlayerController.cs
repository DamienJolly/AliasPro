using AliasPro.API.Players.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Players
{
    public interface IPlayerController
    {
        void LoadCurrencySettings();
        void Cycle();
        ICollection<IPlayer> Players { get; }
        Task<IPlayerData> GetPlayerDataAsync(string SSO);
        Task<IPlayer> GetPlayerAsync(uint playerId);
        Task<IPlayer> GetPlayerByUsernameAsync(string username);
        Task<int> GetPlayerFriendsAsync(uint playerId);
        bool TryGetPlayer(string playerUsername, out IPlayer player);
        bool TryGetPlayer(uint playerId, out IPlayer player);
		Task<uint> TryGetPlayerIdByUsername(string username);

		bool TryAddPlayer(IPlayer player);
        void RemovePlayer(IPlayer player);
        Task UpdatePlayerAsync(IPlayer player);

        Task<IDictionary<int, IPlayerCurrency>> GetPlayerCurrenciesAsync(uint id);
        Task<IDictionary<int, string>> GetPlayerIgnoresAsync(uint id);
        Task UpdatePlayerCurrenciesAsync(IPlayer player);
        Task AddPlayerCurrencyAsync(int playerId, IPlayerCurrency currency);

        Task<IDictionary<string, IPlayerBadge>> GetPlayerBadgesAsync(uint id);
		Task<IDictionary<int, IPlayerBot>> GetPlayerBotsAsync(uint id);
		Task<IDictionary<int, IPlayerPet>> GetPlayerPetsAsync(uint id);
		Task<IDictionary<int, IPlayerAchievement>> GetPlayerAchievementsAsync(uint id);
		Task<IList<IPlayerSanction>> GetPlayerSanctionsAsync(uint id);
        Task AddPlayerSanction(uint playerId, IPlayerSanction sanction);
        Task UpdatePlayerBadgesAsync(IPlayer player);

        Task<IPlayerSettings> GetPlayerSettingsAsync(uint id);
        Task AddPlayerSettingsAsync(uint id);
        Task UpdatePlayerSettingsAsync(IPlayer player);

        Task<ICollection<IPlayerRoomVisited>> GetPlayerRoomVisitsAsync(uint playerId);
		Task<IDictionary<uint, IPlayerData>> GetPlayersByUsernameAsync(string playerName);
		Task AddPlayerIgnoreAsync(int playerId, int targetId);
		Task RemovePlayerIgnoreAsync(int playerId, int targetId);
		Task RemoveFavoriteGroup(int playerId, int groupId);
	}
}
