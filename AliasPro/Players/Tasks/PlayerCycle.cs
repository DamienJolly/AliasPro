using AliasPro.API.Players.Models;
using AliasPro.API.Settings;
using AliasPro.API.Tasks;
using AliasPro.Tasks;
using System.Threading;

namespace AliasPro.Players.Tasks
{
    public class PlayerCycle : ITask
    {
        private readonly CancellationTokenSource _cancellationToken;
        private readonly ISettingsController _settingsController;
        private readonly IPlayer _player;

        public PlayerCycle(ISettingsController settingsController, IPlayer player)
        {
            _settingsController = settingsController;
            _player = player;

            _cancellationToken = new CancellationTokenSource();
        }

        public async void SetupPlayerCycle()
        {
            await TaskHandler.PeriodicAsyncTaskWithDelay(this, 900000, _cancellationToken.Token);
        }

        public void StopPlayerCycle()
        {
            using (_cancellationToken)
            {
                _cancellationToken.Cancel();
            }
        }

        public void Run()
        {
            try
            {
                System.Console.WriteLine("Player: " + _player.Username);
            }
            catch
            {

            }
        }
    }
}
