using AliasPro.API.Chat;
using AliasPro.Sessions;

namespace AliasPro.Chat
{
    internal class ChatController : IChatController
    {
        private readonly ChatRepostiory _chatRepostiory;

        public ChatController(ChatRepostiory chatRepostiory)
        {
            _chatRepostiory = chatRepostiory;
        }

        public bool HandleCommand(ISession session, string message) =>
            _chatRepostiory.HandleCommand(session, message);
    }
}
