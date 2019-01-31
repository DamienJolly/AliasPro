﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Room.Models.Entities
{
    using Network.Protocol;
    using Packets.Outgoing;

    internal class EntityHandler : IDisposable
    {
        internal IDictionary<int, BaseEntity> Entities { get; private set; }
        internal bool HasUserEntities => Entities.Where(x => x.Value is UserEntity).Any();

        private readonly EntityCycler _entityCycler;

        internal EntityHandler(IRoom room)
        {
            Entities = new Dictionary<int, BaseEntity>();
            _entityCycler = new EntityCycler(room);
        }
        
        internal async void Cycle(DateTimeOffset timeOffset)
        {
            foreach (BaseEntity entity in Entities.Values)
            {
                _entityCycler.Cycle(entity);
            }

            await SendAsync(new EntityUpdateComposer(Entities.Values));
        }

        internal async Task SendAsync(ServerPacket packet)
        {
            foreach (BaseEntity entity in Entities.Values)
            {
                if (entity is UserEntity userEntity)
                {
                    await userEntity.Session.WriteAndFlushAsync(packet);
                }
            }
        }

        internal void RemoveEntity(int entityId)
        {
            if (Entities.ContainsKey(entityId))
            {
                Entities.Remove(entityId);
            }
        }

        internal void AddEntity(BaseEntity entity)
        {
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
