using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Rooms.Components
{
    public class EntitiesComponent
    {
        private readonly IRoom _room;
        private readonly IDictionary<int, BaseEntity> _entities;

        public int NextEntitityId = 1;

        public EntitiesComponent(IRoom room)
        {
            _room = room;

            _entities = new Dictionary<int, BaseEntity>();
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
