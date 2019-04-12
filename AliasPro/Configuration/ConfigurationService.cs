using AliasPro.API.Configuration;
using AliasPro.API.Network;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Configuration
{
    internal class ConfigurationService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<ConfigurationRepostiory>();
            collection.AddSingleton<IConfigurationController, ConfigurationController>();
        }
    }
}
