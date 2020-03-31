using AliasPro.API.Sessions.Models;
using System.Threading.Tasks;

namespace AliasPro.API.Chat.Commands
{
    public interface IChatCommand
    {
        string[] Names { get; }
        string PermissionRequired { get; }
        string Parameters { get; }
        string Description { get; }
        Task<bool> Handle(ISession session, string[] args);
    }
}
