using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace AliasPro.Rooms.Tasks
{
    public class RoomTask: IDisposable
    {
        private readonly IRoom _room;
        private CancellationTokenSource _cancellationToken;
        private ActionBlock<DateTimeOffset> _task;

        public RoomTask(IRoom room)
        {
            _room = room;
        }

        public void SetupRoomCycle()
        {
            _cancellationToken = new CancellationTokenSource();

            _task = TaskHandler.PeriodicTaskWithDelay(now => Run(), _cancellationToken.Token, 500);
            _task.Post(DateTimeOffset.Now);
        }

        public async void Run()
        {
            try
            {
                foreach (BaseEntity entity in _room.Entities.Entities)
                    entity.RoomEntityTask.Run();

                foreach (IItem item in _room.Items.Items)
                    item.Interaction.OnCycle();

                await _room.SendAsync(new EntityUpdateComposer(_room.Entities.Entities));
            }
            catch { }
        }

        public void Dispose()
        {
            _cancellationToken.Cancel();
            _task = null;
            _cancellationToken = null;
        }
    }
}
