using System.Threading.Tasks;

namespace AliasPro.Player
{
    using Models;

    public interface IPlayerController
    {
        Task AddPlayerSettingsAsync(uint id);
        Task<IPlayer> GetPlayerByIdAsync(uint id);
        Task<IPlayer> GetPlayerBySsoAsync(string sso);
        Task<IPlayerSettings> GetPlayerSettingsByIdAsync(uint id);
    }
}
