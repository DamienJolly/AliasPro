using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models.Entities;
using AliasPro.Rooms.Packets.Composers;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Rooms.Components
{
    public class EntitiesComponent
    {
        private readonly IRoom _room;
        private readonly EntityCycler _entityCycler;
        private readonly IDictionary<int, BaseEntity> _entities;

        public int NextEntitityId = 1;

        public EntitiesComponent(IRoom room)
        {
            _room = room;

            _entities = new Dictionary<int, BaseEntity>();

            //todo: remove
            _entityCycler = new EntityCycler(_room);
        }

        //todo: remove
        public async void Cycle()
        {
            try
            {
                foreach (BaseEntity entity in _entities.Values)
                {
                    _entityCycler.Cycle(entity);
                }

                await _room.SendAsync(new EntityUpdateComposer(Entities));
            }
            catch { }
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
                if (entity is PlayerEntity)
                    _room.UsersNow++;

                _entities.Add(entity.Id, entity);
            }
        }

        public void RemoveEntity(BaseEntity entity)
        {
            if (_entities.ContainsKey(entity.Id))
            {
                if (entity is PlayerEntity)
                    _room.UsersNow--;

                _entities.Remove(entity.Id);
            }
        }

        public bool TryGetEntityById(int entityId, out BaseEntity entity) =>
            _entities.TryGetValue(entityId, out entity);

        public ICollection<BaseEntity> Entities =>
            _entities.Values;

        public bool HasUserEntities =>
            _entities.Where(x => x.Value is PlayerEntity).Any();
    }
}
