using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Configuration
{
    using Network;

    internal class ConfigurationService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<ConfigurationRepostiory>();
            collection.AddSingleton<IConfigurationController, ConfigurationController>();
        }
    }
}
