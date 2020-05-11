using AliasPro.API.Sessions.Models;
using System.Threading.Tasks;

namespace AliasPro.Game.Chat.Commands
{
    public interface ICommand
    {
        string[] Names { get; }
        string PermissionRequired { get; }
        string Parameters { get; }
        string Description { get; }
        Task<bool> TryHandle(ISession session, string[] args);
    }
}
