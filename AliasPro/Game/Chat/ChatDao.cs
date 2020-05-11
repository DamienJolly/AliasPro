using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.Game.Chat.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Game.Chat
{
	public class ChatDao : BaseDao
    {
        public ChatDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
            : base(logger, configurationController)
        {

        }

        public async Task<List<ChatLog>> ReadUserChatlogs(uint playerId)
        {
            List<ChatLog> logs = new List<ChatLog>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        ChatLog log = new ChatLog(
                            reader.ReadData<int>("player_id"),
                            reader.ReadData<string>("username"),
                            reader.ReadData<int>("target_id"),
                            reader.ReadData<int>("timestamp"),
                            reader.ReadData<string>("message"),
                            reader.ReadData<int>("room_id")
                        );
                        logs.Add(log);
                    }
                }, "SELECT `chatlogs`.* , `players`.`username` FROM `chatlogs` " +
                "INNER JOIN `players` ON `players`.`id` = `chatlogs`.`player_id` " +
                "WHERE `chatlogs`.`player_id` = @0 ORDER BY `chatlogs`.`timestamp` DESC LIMIT 150;", playerId);
            });
            return logs;
        }

        public async Task<List<ChatLog>> ReadRoomChatlogs(uint roomId, int enterTimestamp, int exitTimestamp)
        {
            List<ChatLog> logs = new List<ChatLog>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        ChatLog log = new ChatLog(
                            reader.ReadData<int>("player_id"),
                            reader.ReadData<string>("username"),
                            reader.ReadData<int>("target_id"),
                            reader.ReadData<int>("timestamp"),
                            reader.ReadData<string>("message"),
                            reader.ReadData<int>("room_id")
                        );
                        logs.Add(log);
                    }
                }, "SELECT `chatlogs`.* , `players`.`username` FROM `chatlogs` " +
                "INNER JOIN `players` ON `players`.`id` = `chatlogs`.`player_id` " +
                "WHERE `chatlogs`.`room_id` = @0 AND `chatlogs`.`timestamp` >= @1 AND `chatlogs`.`timestamp` <= @2 " +
                "ORDER BY `chatlogs`.`timestamp` DESC LIMIT 150;", roomId, enterTimestamp, exitTimestamp);
            });
            return logs;
        }

        public async Task<List<ChatLog>> ReadMessengerChatlogs(uint playerId, uint targetId)
        {
            List<ChatLog> logs = new List<ChatLog>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        ChatLog log = new ChatLog(
                            reader.ReadData<int>("player_id"),
                            reader.ReadData<string>("username"),
                            reader.ReadData<int>("target_id"),
                            reader.ReadData<int>("timestamp"),
                            reader.ReadData<string>("message")
                        );
                        logs.Add(log);
                    }
                }, "SELECT `chatlogs_private`.* , `players`.`username` FROM `chatlogs_private` " +
                "INNER JOIN `players` ON `players`.`id` = `chatlogs_private`.`player_id` " +
                "WHERE `chatlogs_private`.`player_id` = @0 AND `chatlogs_private`.`target_id` = @1 " +
                "OR `chatlogs_private`.`target_id` = @0 AND `chatlogs_private`.`player_id` = @1 " +
                "ORDER BY `chatlogs_private`.`timestamp` DESC LIMIT 150;", playerId, targetId);
            });
            return logs;
        }

        public async Task AddUserChatlog(ChatLog chatLog)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `chatlogs` (`player_id`, `target_id`, `room_id`, `message`, `timestamp`, `shouting`) VALUES (@0, @1, @2, @3, @4)",
                    chatLog.PlayerId, chatLog.TargetId, chatLog.RoomId, chatLog.Message, chatLog.Timestamp);
            });
        }

        public async Task AddMessengerChatlog(ChatLog chatLog)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `chatlogs_private` (`player_id`, `target_id`, `message`, `timestamp`) VALUES (@0, @1, @2, @3)",
                    chatLog.PlayerId, chatLog.TargetId, chatLog.Message, chatLog.Timestamp);
            });
        }
    }
}
