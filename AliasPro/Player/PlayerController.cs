using System.Threading.Tasks;

namespace AliasPro.Player
{
    using Models;

    internal class PlayerController : IPlayerController
    {
        private readonly PlayerRepostiory _playerRepostiory;

        public PlayerController(PlayerRepostiory playerRepostiory)
        {
            _playerRepostiory = playerRepostiory;
        }

        public async Task AddPlayerSettingsAsync(uint id) =>
            await _playerRepostiory.CreatePlayerSettings(id);

        public async Task<IPlayer> GetPlayerByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerById(id);

        public async Task<IPlayer> GetPlayerBySsoAsync(string sso) =>
            await _playerRepostiory.GetPlayerBySso(sso);

        public async Task<IPlayerSettings> GetPlayerSettingsByIdAsync(uint id) =>
            await _playerRepostiory.GetPlayerSettingsById(id);
        
        public async Task UpdatePlayerSettingsAsync(uint id, IPlayerSettings settings) =>
            await _playerRepostiory.UpdatePlayerSettings(id, settings);
    }

    public interface IPlayerController
    {
        Task AddPlayerSettingsAsync(uint id);
        Task<IPlayer> GetPlayerByIdAsync(uint id);
        Task<IPlayer> GetPlayerBySsoAsync(string sso);
        Task<IPlayerSettings> GetPlayerSettingsByIdAsync(uint id);
        Task UpdatePlayerSettingsAsync(uint id, IPlayerSettings settings);
    }
}
