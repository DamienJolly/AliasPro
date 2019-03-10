namespace AliasPro.Settings
{
    using Configuration;
    using Database;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class SettingsDao : BaseDao
    {
        public SettingsDao(IConfigurationController configurationController)
           : base(configurationController)
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
    }
}