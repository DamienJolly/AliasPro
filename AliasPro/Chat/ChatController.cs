using AliasPro.API.Chat;
using AliasPro.API.Chat.Commands;
using AliasPro.API.Chat.Models;
using AliasPro.API.Sessions.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Chat
{
    internal class ChatController : IChatController
    {
        private readonly ChatDao _chatDao;
        private readonly IDictionary<string, IChatCommand> _commands;

        public ChatController(ChatDao chatDao, IEnumerable<IChatCommand> commands)
        {
            _chatDao = chatDao;
            _commands = commands.ToDictionary(x => x.Name, x => x);
        }

        public bool HandleCommand(ISession session, string message)
        {
            if (!message.StartsWith(":"))
                return false;

            message = message.Substring(1, message.Length - 1);
            string[] messageData = message.Split(' ');

            if (messageData.Length <= 0)
                return false;

            string commandName = messageData[0].ToLower();
            messageData = messageData.Skip(1).ToArray();

            if (!_commands.TryGetValue(commandName, out IChatCommand command))
                return false;

            return command.Handle(session, messageData);
        }

        public async Task<ICollection<IChatLog>> ReadRoomChatlogs(uint roomId) =>
            await _chatDao.ReadRoomChatlogs(roomId);

        public async Task<ICollection<IChatLog>> ReadUserChatlogs(uint playerId) =>
            await _chatDao.ReadUserChatlogs(playerId);
    }
}
