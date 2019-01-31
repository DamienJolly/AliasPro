using System;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Room.Models.Entities
{
    internal class EntityHandler : IDisposable
    {
        internal IDictionary<int, BaseEntity> Entities { get; private set; }
        internal bool HasUserEntities => Entities.Where(x => x.Value is UserEntity).Any();

        internal EntityHandler(IRoom room)
        {
            Entities = new Dictionary<int, BaseEntity>();
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
