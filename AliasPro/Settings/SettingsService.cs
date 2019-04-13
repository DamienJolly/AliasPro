using AliasPro.API.Network;
using AliasPro.API.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Settings
{
    internal class SettingsService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<SettingsDao>();
            collection.AddSingleton<SettingsRepository>();
            collection.AddSingleton<ISettingsController, SettingsController>();
        }
    }
}
