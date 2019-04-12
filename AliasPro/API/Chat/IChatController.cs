using AliasPro.Sessions;

namespace AliasPro.API.Chat
{
    public interface IChatController
    {
        bool HandleCommand(ISession session, string message);
    }
}
