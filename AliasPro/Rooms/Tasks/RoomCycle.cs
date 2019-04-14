using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Tasks;
using System.Threading;

namespace AliasPro.Rooms.Tasks
{
    public class RoomCycle : ITask
    {
        private readonly IRoom _room;
        private readonly CancellationTokenSource _cancellationToken;
        
        public RoomCycle(IRoom room)
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

        public void Run()
        {
            try
            {
                _room.Entities.Cycle();
                _room.Items.Cycle();
            }
            catch
            {
                // room crashed
            }
        }
    }
}
