using AliasPro.API.Chat.Models;
using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.Chat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Chat
{
    internal class ChatDao : BaseDao
    {
        public ChatDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

        public async Task<ICollection<IChatLog>> ReadRoomChatlogs(uint roomId)
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
                }, "SELECT `chatlogs`.* , `players`.`username` FROM `chatlogs` " +
                "INNER JOIN `players` ON `players`.`id` = `chatlogs`.`player_id` " +
                "WHERE `room_id` = @0 ORDER BY `timestamp` DESC LIMIT 150;", roomId);
            });
            return logs;
        }
    }
}
