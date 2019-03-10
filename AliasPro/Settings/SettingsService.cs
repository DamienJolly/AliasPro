using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Settings
{
    using Network;

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
