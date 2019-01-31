using System.Threading.Tasks;

namespace AliasPro.Player
{
    using Models;

    public interface IPlayerController
    {
        Task<IPlayer> GetPlayerByIdAsync(uint id);
        Task<IPlayer> GetPlayerBySsoAsync(string sso);
    }
}
