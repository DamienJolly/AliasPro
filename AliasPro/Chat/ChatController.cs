using AliasPro.API.Chat;
using AliasPro.API.Chat.Commands;
using AliasPro.API.Chat.Models;
using AliasPro.API.Permissions;
using AliasPro.API.Sessions.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Chat
{
    internal class ChatController : IChatController
    {
        private readonly IPermissionsController _permissionsController;
        private readonly ChatDao _chatDao;
        private readonly IDictionary<string, IChatCommand> _commands;

        public ChatController(
            IPermissionsController permissionsController,
            ChatDao chatDao, 
            IEnumerable<IChatCommand> commands)
        {
            _permissionsController = permissionsController;
            _chatDao = chatDao;
            _commands = new Dictionary<string, IChatCommand>();

            foreach (IChatCommand command in commands)
            {
                foreach (string name in command.Names)
                {
                    _commands.TryAdd(name, command);
                }
            }
        }

        public async Task<bool> HandleCommand(ISession session, string message)
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

            if (!_permissionsController.HasPermission(session.Player, command.PermissionRequired))
                return false;

            return await command.Handle(session, messageData);
        }
        
        public async Task<ICollection<IChatLog>> ReadUserChatlogs(uint playerId) =>
            await _chatDao.ReadUserChatlogs(playerId);

        public async Task<ICollection<IChatLog>> ReadRoomChatlogs(uint roomId, int enterTimestamp, int exitTimestamp) =>
            await _chatDao.ReadRoomChatlogs(roomId, enterTimestamp, exitTimestamp);

        public async Task<ICollection<IChatLog>> ReadMessengerChatlogs(uint playerId, uint targetId) =>
            await _chatDao.ReadMessengerChatlogs(playerId, targetId);

        public async Task AddUserChatlog(IChatLog chatLog) =>
            await _chatDao.AddUserChatlog(chatLog);

        public async Task AddMessengerChatlog(IChatLog chatLog) =>
            await _chatDao.AddMessengerChatlog(chatLog);

        public IList<IChatCommand> CommandsForRank(ISession session)
        {
            IList<IChatCommand> commands = new List<IChatCommand>();
            foreach (IChatCommand command in _commands.Values)
            {
                if (!_permissionsController.HasPermission(session.Player, command.PermissionRequired))
                    continue;

                if (commands.Contains(command))
                    continue;

                commands.Add(command);
            }
            return commands;
        }
    }
}
