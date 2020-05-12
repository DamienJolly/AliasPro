using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Game.Settings
{
	public class SettingsController
	{
		private readonly ILogger<SettingsController> logger;
		private readonly SettingsDao settingsDao;

		private Dictionary<string, string> settings;

		public SettingsController(
			ILogger<SettingsController> logger,
			SettingsDao settingsDao)
		{
			this.logger = logger;
			this.settingsDao = settingsDao;

			settings = new Dictionary<string, string>();

			LoadEmulatorSettings();

			this.logger.LogInformation("Loaded " + settings.Count + " server settings.");
		}

		public async void LoadEmulatorSettings()
		{
			settings = await settingsDao.GetEmulatorSettings();
		}

		public async Task CleanupDatabase()
		{
			//todo: more cleanup
			await settingsDao.CleanupPlayers();
		}

		public string GetString(string key) =>
			settings.GetValueOrDefault(key);

		public int GetInt(string key)
		{
			int.TryParse(settings.GetValueOrDefault(key), out int number);
			return number;
		}

		public bool GetBool(string key) =>
			settings.GetValueOrDefault(key) == "1";
	}
}
