using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Game.Settings
{
	public class SettingsService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<SettingsDao>();
            collection.AddSingleton<SettingsController>();
        }
    }
}
