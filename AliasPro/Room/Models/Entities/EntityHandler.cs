using System;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Room.Models.Entities
{
    using Packets.Outgoing;
    using Gamemap;

    public class EntityHandler
    {
        private readonly IRoom _room;
        private readonly EntityCycler _entityCycler;
        private readonly IDictionary<int, BaseEntity> _entities;

        public int NextEntitityId = 1;
        
        public EntityHandler(IRoom room)
        {
            _entities = new Dictionary<int, BaseEntity>();
            _room = room;
            _entityCycler = new EntityCycler(_room);
        }

        public async void Cycle(DateTimeOffset timeOffset)
        {
            foreach (BaseEntity entity in Entities)
            {
                _entityCycler.Cycle(entity);
            }

            try
            {
                await _room.SendAsync(new EntityUpdateComposer(Entities));
            } catch { }
        }

        public async void Unidle(BaseEntity entity)
        {
            entity.IdleTimer = 0;
            entity.IsIdle = false;
            await _room.SendAsync(new UserSleepComposer(entity));
        }
        
        public void AddEntity(BaseEntity entity)
        {
            if (!_entities.ContainsKey(entity.Id))
            {
                if (entity is UserEntity)
                    _room.RoomData.UsersNow++;

                _entities.Add(entity.Id, entity);
            }
        }

        public void RemoveEntity(BaseEntity entity)
        {
            if (_entities.ContainsKey(entity.Id))
            {
                if (entity is UserEntity)
                    _room.RoomData.UsersNow--;

                _entities.Remove(entity.Id);
            }
        }

        public bool TryGetEntityById(int entityId, out BaseEntity entity) =>
            _entities.TryGetValue(entityId, out entity);

        public ICollection<BaseEntity> Entities =>
            _entities.Values;

        public bool HasUserEntities =>
            _entities.Where(x => x.Value is UserEntity).Any();
    }
}
