using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace AliasPro.Room.Models
{
    using Entities;
    using Gamemap;
    using Tasks;
    using Sessions;
    using System.Threading.Tasks;
    using AliasPro.Network.Protocol;
    using AliasPro.Room.Packets.Outgoing;
    using AliasPro.Network.Events;

    internal class Room : IRoom, IDisposable
    {
        private readonly EntityHandler _entityHandler;
        private ActionBlock<DateTimeOffset> task;

        internal Room(IRoomData roomData, IRoomModel model)
        {
            RoomData = roomData;
            RoomModel = model;

            SetupRoomCycle();

            RoomMap = new RoomMap(RoomModel);
            _entityHandler = new EntityHandler(this);
        }

        public RoomMap RoomMap { get; set; }
        public IRoomData RoomData { get; set; }
        public IRoomModel RoomModel { get; set; }
        public IDictionary<int, BaseEntity> Entities => _entityHandler.Entities;
        
        public async void OnChat(string text, int colour, BaseEntity entity)
        {
            if (colour == 1 || colour == -1 || colour == 2)
            {
                colour = 0;
            }

            if (text.Length > 100)
            {
                text = text.Substring(0, 100);
            }

            await SendAsync(new AvatarChatComposer(entity.Id, text, 0, colour));
        }

        public void LeaveRoom(ISession session)
        {
            int entityId = session.Entity.Id;
            _entityHandler.RemoveEntity(entityId);
            session.Entity = null;
            session.CurrentRoom = null;

            if (!_entityHandler.HasUserEntities)
            {
                //Todo: unload room
                //StopRoomCycle();
            }
        }
        
        private void SetupRoomCycle()
        {
            task = TaskHandler.PeriodicTaskWithDelay(time => Cycle(time), 500);
            task.Post(DateTimeOffset.Now);
        }

        private void StopRoomCycle()
        {
            task = null;
        }

        private void Cycle(DateTimeOffset timeOffset)
        {
            _entityHandler.Cycle(timeOffset);
        }

        public Task SendAsync(IPacketComposer serverPacket) => _entityHandler.SendAsync(serverPacket);

        public Task AddEntity(BaseEntity entity) => _entityHandler.AddEntity(entity);

        public void Dispose()
        {
            StopRoomCycle();
            _entityHandler.Dispose();
        }
    }
}
