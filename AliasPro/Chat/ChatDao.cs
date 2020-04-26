using AliasPro.API.Chat.Models;
using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.Chat.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Chat
{
    internal class ChatDao : BaseDao
    {
        public ChatDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
            : base(logger, configurationController)
        {

        }

        public async Task<ICollection<IChatLog>> ReadUserChatlogs(uint playerId)
        {
            ICollection<IChatLog> logs = new List<IChatLog>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IChatLog log = new ChatLog(reader)
                        {
                            RoomId = reader.ReadData<int>("room_id")
                        };
                        logs.Add(new ChatLog(reader));
                    }
                }, "SELECT `chatlogs`.* , `players`.`username` FROM `chatlogs` " +
                "INNER JOIN `players` ON `players`.`id` = `chatlogs`.`player_id` " +
                "WHERE `chatlogs`.`player_id` = @0 ORDER BY `chatlogs`.`timestamp` DESC LIMIT 150;", playerId);
            });
            return logs;
        }

        public async Task<ICollection<IChatLog>> ReadRoomChatlogs(uint roomId, int enterTimestamp, int exitTimestamp)
        {
            ICollection<IChatLog> logs = new List<IChatLog>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IChatLog log = new ChatLog(reader)
                        {
                            RoomId = reader.ReadData<int>("room_id")
                        };
                        logs.Add(new ChatLog(reader));
                    }
                }, "SELECT `chatlogs`.* , `players`.`username` FROM `chatlogs` " +
                "INNER JOIN `players` ON `players`.`id` = `chatlogs`.`player_id` " +
                "WHERE `chatlogs`.`room_id` = @0 AND `chatlogs`.`timestamp` >= @1 AND `chatlogs`.`timestamp` <= @2 " +
                "ORDER BY `chatlogs`.`timestamp` DESC LIMIT 150;", roomId, enterTimestamp, exitTimestamp);
            });
            return logs;
        }

        public async Task<ICollection<IChatLog>> ReadMessengerChatlogs(uint playerId, uint targetId)
        {
            ICollection<IChatLog> logs = new List<IChatLog>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        logs.Add(new ChatLog(reader));
                    }
                }, "SELECT `chatlogs_private`.* , `players`.`username` FROM `chatlogs_private` " +
                "INNER JOIN `players` ON `players`.`id` = `chatlogs_private`.`player_id` " +
                "WHERE `chatlogs_private`.`player_id` = @0 AND `chatlogs_private`.`target_id` = @1 " +
                "OR `chatlogs_private`.`target_id` = @0 AND `chatlogs_private`.`player_id` = @1 " +
                "ORDER BY `chatlogs_private`.`timestamp` DESC LIMIT 150;", playerId, targetId);
            });
            return logs;
        }

        public async Task AddUserChatlog(IChatLog chatLog)
        {
            ICollection<IChatLog> logs = new List<IChatLog>();
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `chatlogs` (`player_id`, `target_id`, `room_id`, `message`, `timestamp`, `shouting`) VALUES (@0, @1, @2, @3, @4)",
                    chatLog.PlayerId, chatLog.TargetId, chatLog.RoomId, chatLog.Message, chatLog.Timestamp);
            });
        }

        public async Task AddMessengerChatlog(IChatLog chatLog)
        {
            ICollection<IChatLog> logs = new List<IChatLog>();
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `chatlogs_private` (`player_id`, `target_id`, `message`, `timestamp`) VALUES (@0, @1, @2, @3)",
                    chatLog.PlayerId, chatLog.TargetId, chatLog.Message, chatLog.Timestamp);
            });
        }
    }
}
