using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Chat.Models
{
    using Sessions;
    using Commands;

    internal class CommandHandler
    {
        private readonly IDictionary<string, IChatCommand> _commands;

        public CommandHandler(IEnumerable<IChatCommand> commands)
        {
            _commands = commands.ToDictionary(x => x.Name, x => x);
        }

        public async Task<bool> Handle(ISession session, string message)
        {
            if (!message.StartsWith(":"))
                return false;

            message = message.Substring(1, message.Length - 1);
            string[] messageData = message.Split(' ');

            if (messageData.Length <= 0)
                return false;

            string commandName = messageData[0].ToLower();
            messageData = messageData.Skip(1).ToArray();

            if (_commands.TryGetValue(commandName, out IChatCommand command))
            {

                await command.Handle(session, messageData);
                return true;
            }

            return false;
        }
    }
}
