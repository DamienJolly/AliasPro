using AliasPro.Player.Models;
using System.Threading.Tasks;

namespace AliasPro.Player
{
    public interface IPlayerController
    {
        Task<IPlayer> GetPlayerByIdAsync(uint id);
        Task<IPlayer> GetPlayerBySsoAsync(string sso);
    }
}
