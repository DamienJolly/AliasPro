namespace AliasPro.Configuration
{
    using Models;

    internal class ConfigurationController : IConfigurationController
    {
        private readonly ConfigurationRepostiory _configurationRepostiory;

        public ConfigurationController(ConfigurationRepostiory configurationRepostiory)
        {
            _configurationRepostiory = configurationRepostiory;
        }

        public IConfigurationData GetConfigurationData() =>
            _configurationRepostiory.ConfigurationData;
    }

    public interface IConfigurationController
    {
        IConfigurationData GetConfigurationData();
    }
}
