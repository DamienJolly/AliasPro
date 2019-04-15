using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Tasks;
using System.Threading;

namespace AliasPro.Rooms.Tasks
{
    public class RoomTask : ITask
    {
        private readonly IRoom _room;
        private readonly CancellationTokenSource _cancellationToken;
        
        public RoomTask(IRoom room)
        {
            _room = room;
            _cancellationToken = new CancellationTokenSource();
        }

        public async void SetupRoomCycle()
        {
            await TaskHandler.PeriodicAsyncTaskWithDelay(this, 500, _cancellationToken.Token);
        }

        public void StopRoomCycle()
        {
            using (_cancellationToken)
            {
                _cancellationToken.Cancel();
            }
        }

        public async void Run()
        {
            await TaskHandler.RunTaskAsync(new EntitiesTask(_room));
            await TaskHandler.RunTaskAsync(new ItemsTask(_room));
        }
    }
}
