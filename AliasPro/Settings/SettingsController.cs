using AliasPro.API.Settings;
using System.Threading.Tasks;

namespace AliasPro.Settings
{
    internal class SettingsController : ISettingsController
    {
        private readonly SettingsRepository _settingsRepository;

        public SettingsController(SettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public string GetSetting(string key) =>
            _settingsRepository.GetSetting(key);

        public async Task CleanupDatabase() =>
            await _settingsRepository.CleanupDatabase();
    }
}
