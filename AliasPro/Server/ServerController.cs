using AliasPro.API.Rooms;
using AliasPro.API.Server;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace AliasPro.Server
{
    internal class ServerController : IServerController
    {
        private readonly IRoomController _roomController;
        private readonly ServerDao _serverDao;
        private readonly Timer _serverCycleTimer;
        private IDictionary<string, string> _settings;

        public ServerController(ServerDao serverDao, IRoomController roomController)
        {
            _roomController = roomController;
            _serverDao = serverDao;
            _settings = new Dictionary<string, string>();

            _serverCycleTimer = new Timer();
            _serverCycleTimer.Elapsed += Cycle;
            _serverCycleTimer.AutoReset = true;
            _serverCycleTimer.Interval = 500;
            _serverCycleTimer.Start();

            LoadEmulatorSettings();
        }

        private void Cycle(object sender, ElapsedEventArgs e)
        {
            try
            {
                _roomController.Cycle();
            }
            catch { }
        }

        public async void LoadEmulatorSettings()
        {
            if (_settings.Count > 0) _settings.Clear();

            _settings =
                await _serverDao.GetEmulatorSettings();
        }

        public async Task CleanupDatabase()
        {
            //todo: more cleanup
            await _serverDao.CleanupPlayers();
        }

        public string GetSetting(string key) =>
            _settings.ContainsKey(key) ? _settings[key] : "";
    }
}
