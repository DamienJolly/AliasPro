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
    }

    public interface ISettingsController
    {
        string GetSetting(string key);
    }
}
