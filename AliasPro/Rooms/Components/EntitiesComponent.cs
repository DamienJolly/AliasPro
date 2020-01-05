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

        public EntitiesComponent(IRoom room, 
			IDictionary<int, BaseEntity> bots, 
			IDictionary<int, BaseEntity> pets)
        {
            _room = room;
            _entities = new Dictionary<int, BaseEntity>();

			foreach (BaseEntity bot in bots.Values)
			{
				int entityId = NextEntitityId++;
				bot.Id = entityId;
				AddEntity(bot);
			}

			foreach (BaseEntity pet in pets.Values)
			{
				int entityId = NextEntitityId++;
				pet.Id = entityId;
				AddEntity(pet);
			}
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

        public bool TryGetPlayerEntityById(int playerId, out PlayerEntity entity)
        {
            entity = _entities.Values.Where(x => x is PlayerEntity playerEntity && playerEntity.Player.Id == playerId).FirstOrDefault() as PlayerEntity;
            return entity != null;
        }

        public bool TryGetPlayerEntityByName(string username, out BaseEntity entity)
        {
            entity = _entities.Values.Where(x => x is PlayerEntity && x.Name == username).FirstOrDefault();
            return entity != null;
        }

        public bool HasEntity(int entityId) =>
			_entities.ContainsKey(entityId);

        public ICollection<BaseEntity> Entities =>
            _entities.Values;

        public ICollection<BaseEntity> PlayerEntities =>
            _entities.Values.Where(x => x is PlayerEntity).ToList();

        public ICollection<BaseEntity> BotEntities =>
			_entities.Values.Where(x => x is BotEntity).ToList();

		public ICollection<BaseEntity> PetEntities =>
			_entities.Values.Where(x => x is PetEntity).ToList();

		public bool HasUserEntities =>
            _entities.Where(x => x.Value is PlayerEntity).Any();
    }
}
