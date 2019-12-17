using AliasPro.API.Configuration;
using AliasPro.API.Database;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Server
{
    internal class ServerDao : BaseDao
    {
        public ServerDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
            : base(logger, configurationController)
        {

        }

        internal async Task<IDictionary<string, string>> GetEmulatorSettings()
        {
            IDictionary<string, string> settings = new Dictionary<string, string>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        if (!settings.ContainsKey(reader.ReadData<string>("key")))
                        {
                            settings.Add(reader.ReadData<string>("kay"),
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
