using AliasPro.API.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Configuration
{
    internal class ConfigurationService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<ConfigurationRepostiory>();
            collection.AddSingleton<IConfigurationController, ConfigurationController>();
        }
    }
}
