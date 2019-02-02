using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Room.Models.Entities
{
    using AliasPro.Network.Events;
    using Packets.Outgoing;

    public class EntityHandler : IDisposable
    {
        public IDictionary<int, BaseEntity> Entities { get; private set; }
        public bool HasUserEntities => Entities.Where(x => x.Value is UserEntity).Any();

        private readonly EntityCycler _entityCycler;

        public EntityHandler(IRoom room)
        {
            Entities = new Dictionary<int, BaseEntity>();
            _entityCycler = new EntityCycler(room);
        }

        public async void Cycle(DateTimeOffset timeOffset)
        {
            foreach (BaseEntity entity in Entities.Values)
            {
                _entityCycler.Cycle(entity);
            }

            await SendAsync(new EntityUpdateComposer(Entities.Values));
        }

        public async Task SendAsync(IPacketComposer packet)
        {
            foreach (BaseEntity entity in Entities.Values)
            {
                if (entity is UserEntity userEntity)
                {
                    await userEntity.Session.SendPacketAsync(packet);
                }
            }
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
            await SendAsync(new EntitiesComposer(entity));
            await SendAsync(new EntityUpdateComposer(entity));
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
