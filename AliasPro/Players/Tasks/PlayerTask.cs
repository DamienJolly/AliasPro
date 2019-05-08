using AliasPro.API.Players.Models;
using AliasPro.API.Tasks;
using AliasPro.Tasks;
using System.Threading;

namespace AliasPro.Players.Tasks
{
    public class PlayerTask : ITask
    {
        private readonly CancellationTokenSource _cancellationToken;
        private readonly IPlayer _player;

        public PlayerTask(IPlayer player)
        {
            _player = player;

            _cancellationToken = new CancellationTokenSource();
        }

        public void SetupPlayerCycle()
        {

        }

        public void StopPlayerCycle()
        {

        }

        public void Run()
        {
            // todo: credits timers ext
        }
    }
}
