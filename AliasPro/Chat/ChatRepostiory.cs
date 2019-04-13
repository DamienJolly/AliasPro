using AliasPro.API.Chat.Commands;
using AliasPro.API.Sessions.Models;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Chat
{
    internal class ChatRepostiory
    {
        private readonly IDictionary<string, IChatCommand> _commands;

        public ChatRepostiory(IEnumerable<IChatCommand> commands)
        {
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
    }
}
