using System.Threading.Tasks;

namespace AliasPro.Chat
{
    using Sessions;

    internal class ChatController : IChatController
    {
        private readonly ChatRepostiory _chatRepostiory;

        public ChatController(ChatRepostiory chatRepostiory)
        {
            _chatRepostiory = chatRepostiory;
        }

        public async Task<bool> HandleCommandAsync(ISession session, string message) =>
            await _chatRepostiory.HandleCommandAsync(session, message);
    }

    public interface IChatController
    {
        Task<bool> HandleCommandAsync(ISession session, string message);
    }
}
