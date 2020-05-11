using AliasPro.API.Sessions.Models;
using AliasPro.Players.Packets.Composers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliasPro.Game.Chat.Commands
{
    public class CommandsCommand : ICommand
    {
        public string[] Names => new[]
        {
            "commands",
            "cmds"
        };

        public string PermissionRequired => "cmd_commands";

        public string Parameters => "";

        public string Description => "Hello world!";

        public async Task<bool> TryHandle(ISession session, string[] args)
        {
            StringBuilder message = new StringBuilder("This is the list of commands you have available:\n");

            List<ICommand> commands = Program.GetService<ChatController>().GetPlayerCommands(session);
            foreach (ICommand command in commands)
            {
                string param = string.IsNullOrEmpty(command.Parameters) ? "" : " " + command.Parameters;
                message.Append(":" + command.Names[0] + param + " - " + command.Description + "\n");
            }

            await session.SendPacketAsync(new MessagesForYouComposer(message.ToString()));
            return true;
        }
    }
}
