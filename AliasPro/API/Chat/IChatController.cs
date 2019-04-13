using AliasPro.API.Sessions.Models;

namespace AliasPro.API.Chat
{
    public interface IChatController
    {
        bool HandleCommand(ISession session, string message);
    }
}
