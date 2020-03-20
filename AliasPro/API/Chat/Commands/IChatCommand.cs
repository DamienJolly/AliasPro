using AliasPro.API.Sessions.Models;

namespace AliasPro.API.Chat.Commands
{
    public interface IChatCommand
    {
        string[] Names { get; }
        string PermissionRequired { get; }
        string Parameters { get; }
        string Description { get; }
        bool Handle(ISession session, string[] args);
    }
}
