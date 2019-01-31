using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace AliasPro.Room.Models
{
    using Entities;
    using Gamemap;
    using Tasks;

    internal class Room : IRoom, IDisposable
    {
        private readonly EntityHandler _entityHandler;
        private readonly CancellationTokenSource _cancellationToken;
        private ActionBlock<DateTimeOffset> task;

        internal Room(IRoomData roomData, IRoomModel model)
        {
            RoomData = roomData;
            RoomModel = model;

            _cancellationToken = new CancellationTokenSource();
            RoomMap = new RoomMap(RoomModel);
            _entityHandler = new EntityHandler(this);
        }

        public RoomMap RoomMap { get; set; }
        public IRoomData RoomData { get; set; }
        public IRoomModel RoomModel { get; set; }
        public IDictionary<int, BaseEntity> Entities => _entityHandler.Entities;
        public bool CycleActive => _cancellationToken.IsCancellationRequested;

        public void AddEntity(BaseEntity entity) => _entityHandler.AddEntity(entity);

        public void SetupRoomCycle()
        {
            task = TaskHandler.PeriodicTaskWithDelay(time => Cycle(time), _cancellationToken.Token, 500);
            task.Post(DateTimeOffset.Now);
        }

        public void StopRoomCycle()
        {
            using (_cancellationToken)
            {
                _cancellationToken.Cancel();
            }

            task = null;
        }

        private void Cycle(DateTimeOffset timeOffset)
        {
            _entityHandler.Cycle(timeOffset);
        }

        public void Dispose()
        {
            StopRoomCycle();
            _entityHandler.Dispose();
        }
    }
}
