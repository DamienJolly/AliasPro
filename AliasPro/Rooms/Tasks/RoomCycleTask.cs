using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Tasks;
using System.Threading;

namespace AliasPro.Rooms.Tasks
{
    public class RoomCycleTask : ITask
    {
        private readonly IRoom _room;
        private readonly CancellationTokenSource _cancellationToken;
        
        public RoomCycleTask(IRoom room)
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
            foreach (BaseEntity entity in _room.Entities.Entities)
            {
                entity.Cycle();
                await TaskHandler.RunTaskAsync(new RoomEntityWalkTask(entity));
            }
            await _room.SendAsync(new EntityUpdateComposer(_room.Entities.Entities));

            foreach (IItem item in _room.Items.Items)
            {
                item.Interaction.OnCycle();
            }
        }
    }
}
