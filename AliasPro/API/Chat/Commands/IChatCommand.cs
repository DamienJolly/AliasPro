using AliasPro.Sessions;

namespace AliasPro.API.Chat.Commands
{
    interface IChatCommand
    {
        string Name { get; }
        string Description { get; }
        bool Handle(ISession session, string[] args);
    }
}
