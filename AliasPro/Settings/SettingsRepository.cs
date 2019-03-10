﻿using System.Collections.Generic;

namespace AliasPro.Settings
{
    internal class SettingsRepository
    {
        private readonly SettingsDao _settingsDao;
        private IDictionary<string, string> _settings;

        public SettingsRepository(SettingsDao settingsDao)
        {
            _settingsDao = settingsDao;
            _settings = new Dictionary<string, string>();

            LoadEmulatorSettings();
        }

        private async void LoadEmulatorSettings()
        {
            if (_settings.Count > 0) _settings.Clear();

            _settings =
                await _settingsDao.GetEmulatorSettings();
        }

        public string GetSetting(string key) => 
            _settings.ContainsKey(key) ? _settings[key] : "";
    }
}
