using AliasPro.API.Configuration;
using AliasPro.API.Configuration.Models;

namespace AliasPro.Configuration
{
    internal class ConfigurationController : IConfigurationController
    {
        private readonly ConfigurationRepostiory _configurationRepostiory;

        public ConfigurationController(ConfigurationRepostiory configurationRepostiory)
        {
            _configurationRepostiory = configurationRepostiory;
        }

        public IConfigurationData ConfigurationData =>
            _configurationRepostiory.ConfigurationData;
    }
}
