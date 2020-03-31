using AliasPro.API.Chat;
using AliasPro.API.Chat.Commands;
using AliasPro.API.Sessions.Models;
using AliasPro.Players.Packets.Composers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliasPro.Chat.Commands
{
    internal class CommandsCommand : IChatCommand
    {
        public string[] Names => new[]
        {
            "commands",
            "cmds"
        };

        public string PermissionRequired => "cmd_commands";

        public string Parameters => "";

        public string Description => "Hello world!";

        public async Task<bool> Handle(ISession session, string[] args)
        {
            StringBuilder message = new StringBuilder("This is the list of commands you have available:\n");

            IList<IChatCommand> commands = Program.GetService<IChatController>().CommandsForRank(session);
            foreach (IChatCommand command in commands)
            {
                string param = string.IsNullOrEmpty(command.Parameters) ? "" : " " + command.Parameters;
                message.Append(":" + command.Names[0] + param + " - " + command.Description + "\n");
            }

            await session.SendPacketAsync(new MessagesForYouComposer(message.ToString()));
            return true;
        }
    }
}
