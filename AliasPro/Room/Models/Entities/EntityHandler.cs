using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Room.Models.Entities
{
    using Packets.Outgoing;

    public class EntityHandler : IDisposable
    {
        private readonly EntityCycler _entityCycler;
        private readonly IRoom _room;

        public IDictionary<int, BaseEntity> Entities { get; private set; }
        public bool HasUserEntities => Entities.Where(x => x.Value is UserEntity).Any();

        public EntityHandler(IRoom room)
        {
            Entities = new Dictionary<int, BaseEntity>();
            _room = room;
            _entityCycler = new EntityCycler(_room);
        }

        public async void Cycle(DateTimeOffset timeOffset)
        {
            foreach (BaseEntity entity in Entities.Values)
            {
                _entityCycler.Cycle(entity);
            }

            await _room.SendAsync(new EntityUpdateComposer(Entities.Values));
        }

        public void RemoveEntity(int entityId)
        {
            if (Entities.ContainsKey(entityId))
            {
                Entities.Remove(entityId);
            }
        }

        public async Task AddEntity(BaseEntity entity)
        {
            await _room.SendAsync(new EntitiesComposer(entity));
            await _room.SendAsync(new EntityUpdateComposer(entity));
            Entities.Add(entity.Id, entity);
        }

        public void Dispose()
        {
            foreach (BaseEntity entity in Entities.Values)
            {
                //entity.Dispose(); todo: for later
            }

            Entities = null;
        }
    }
}
