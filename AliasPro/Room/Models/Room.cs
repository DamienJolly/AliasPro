using System;
using System.Collections.Generic;

namespace AliasPro.Room.Models
{
    using Entities;

    internal class Room : IRoom, IDisposable
    {
        private readonly EntityHandler _entityHandler;

        internal Room(IRoomData roomData, IRoomModel model)
        {
            RoomData = roomData;
            RoomModel = model;

            _entityHandler = new EntityHandler(this);
        }
        
        public IRoomData RoomData { get; set; }
        public IRoomModel RoomModel { get; set; }
        public IDictionary<int, BaseEntity> Entities => _entityHandler.Entities;

        public void AddEntity(BaseEntity entity) => _entityHandler.AddEntity(entity);

        public void Dispose()
        {
            _entityHandler.Dispose();
        }
    }
}
