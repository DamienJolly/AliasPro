using AliasPro.API.Permissions;
using AliasPro.API.Sessions.Models;
using AliasPro.Game.Chat.Commands;
using AliasPro.Game.Chat.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Game.Chat
{
	public class ChatController
	{
        private readonly ILogger<ChatController> logger;
        private readonly ChatDao chatDao;

        private readonly Dictionary<string, ICommand> commands;

        public ChatController(
            ILogger<ChatController> logger,
            ChatDao chatDao,
            IEnumerable<ICommand> commands)
        {
            this.logger = logger;
            this.chatDao = chatDao;
            this.commands = new Dictionary<string, ICommand>();

            foreach (ICommand command in commands)
            {
                foreach (string name in command.Names)
                {
                    this.commands.TryAdd(name, command);
                }
            }

            this.logger.LogInformation("Loaded " + this.commands.Count + " commands.");
        }

        public async Task<bool> TryHandleCommand(ISession session, string message)
        {
            if (!message.StartsWith(":"))
                return false;

            message = message.Substring(1, message.Length - 1);
            string[] messageData = message.Split(' ');

            if (messageData.Length <= 0)
                return false;

            string commandName = messageData[0].ToLower();
            messageData = messageData.Skip(1).ToArray();

            if (!commands.TryGetValue(commandName, out ICommand command))
                return false;

            if (!Program.GetService<IPermissionsController>().HasPermission(session.Player, command.PermissionRequired))
                return false;

            return await command.TryHandle(session, messageData);
        }

        public List<ICommand> GetPlayerCommands(ISession session)
        {
            List<ICommand> commandsToGo = new List<ICommand>();
            foreach (ICommand command in commands.Values)
            {
                if (!Program.GetService<IPermissionsController>().HasPermission(session.Player, command.PermissionRequired))
                    continue;

                if (commandsToGo.Contains(command))
                    continue;

                commandsToGo.Add(command);
            }
            return commandsToGo;
        }

        public async Task<ICollection<ChatLog>> ReadUserChatlogs(uint playerId) =>
            await chatDao.ReadUserChatlogs(playerId);

        public async Task<ICollection<ChatLog>> ReadRoomChatlogs(uint roomId, int enterTimestamp = 0, int exitTimestamp = int.MaxValue) =>
            await chatDao.ReadRoomChatlogs(roomId, enterTimestamp, exitTimestamp);

        public async Task<ICollection<ChatLog>> ReadMessengerChatlogs(uint playerId, uint targetId) =>
            await chatDao.ReadMessengerChatlogs(playerId, targetId);

        public async Task AddUserChatlog(ChatLog chatLog) =>
            await chatDao.AddUserChatlog(chatLog);

        public async Task AddMessengerChatlog(ChatLog chatLog) =>
            await chatDao.AddMessengerChatlog(chatLog);
    }
}
