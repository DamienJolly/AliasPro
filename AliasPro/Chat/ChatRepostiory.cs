using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Chat
{
    using Sessions;
    using Models;
    using Models.Commands;

    internal class ChatRepostiory
    {
        private readonly CommandHandler _commandHandler;
        private readonly ChatDao _chatDao;

        public ChatRepostiory(ChatDao chatDao, IEnumerable<IChatCommand> commands)
        {
            _chatDao = chatDao;
            _commandHandler = new CommandHandler(commands);
        }

        public async Task<bool> HandleCommandAsync(ISession session, string message) =>
            await _commandHandler.Handle(session, message);
    }
}
