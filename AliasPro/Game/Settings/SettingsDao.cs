using AliasPro.API.Database;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Game.Settings
{
	public class SettingsDao : BaseDao
    {
        public SettingsDao(ILogger<BaseDao> logger)
            : base(logger)
        {

        }

        internal async Task<Dictionary<string, string>> GetEmulatorSettings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        if (!settings.ContainsKey(reader.ReadData<string>("key")))
                        {
                            settings.Add(reader.ReadData<string>("key"),
                                reader.ReadData<string>("value"));
                        }
                    }
                }, "SELECT `key`, `value` FROM `server_settings`;");
            });

            return settings;
        }

        internal async Task CleanupPlayers()
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `players` SET `is_online` = '0';");
            });
        }
    }
}
