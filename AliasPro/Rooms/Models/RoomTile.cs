using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;
using System.Collections.Generic;

namespace AliasPro.Rooms.Models
{
    internal class RoomTile : IRoomTile
    {
        private readonly IRoom _room;
        private readonly IRoomPosition _position;
        private readonly IDictionary<uint, IItem> _items;
        private readonly IDictionary<int, BaseEntity> _entities;

        internal RoomTile(IRoom room, IRoomPosition position)
        {
            _room = room;
            _position = position;
            _items = new Dictionary<uint, IItem>();
            _entities = new Dictionary<int, BaseEntity>();
        }

        public bool IsValidTile(BaseEntity entity, bool final)
        {
            if (_entities.Count > 0)
                return entity == null ? false : _entities.ContainsKey(entity.Id);

            IItem topItem = TopItem;
            if (topItem != null)
            {
                if (topItem.ItemData.CanWalk) return true;

                if (topItem.ItemData.InteractionType == ItemInteractionType.CHAIR && final) return true;

                if (topItem.ItemData.InteractionType == ItemInteractionType.BED && final) return true;

                return false;
            }

            return true;
        }

        public bool CanRoll(IItem item)
        {
            IItem topItem = TopItem;

            if (topItem == item) return true;

            if (_entities.Count > 0) return false;

            if (topItem != null)
                return topItem.ItemData.CanWalk;

            return true;
        }

        public bool CanStack(IItem item)
        {
            IItem topItem = TopItem;

            if (topItem == item) return true;

            if (_entities.Count > 0) return false;

            if (topItem != null)
                return topItem.ItemData.CanStack;

            return true;
        }

        public void AddItem(IItem item)
        {
            if (!_items.ContainsKey(item.Id))
            {
                _items.Add(item.Id, item);
            }
        }

        public void RemoveItem(uint itemId) =>
            _items.Remove(itemId);

        public void AddEntity(BaseEntity entity)
        {
            if (!_entities.ContainsKey(entity.Id))
            {
                _entities.Add(entity.Id, entity);
            }
        }

        public void RemoveEntity(int entityId) =>
            _entities.Remove(entityId);

        public ICollection<BaseEntity> Entities =>
            _entities.Values;

        public ICollection<IItem> WiredEffects
        {
            get
            {
                IList<IItem> effects = new List<IItem>();
                foreach (IItem item in _items.Values)
                {
                    if (item.ItemData.InteractionType == ItemInteractionType.WIRED_EFFECT)
                        effects.Add(item);
                }
                return effects;
            }
        }

        public ICollection<IItem> WiredConditions
        {
            get
            {
                IList<IItem> conditions = new List<IItem>();
                foreach (IItem item in _items.Values)
                {
                    if (item.ItemData.InteractionType == ItemInteractionType.WIRED_CONDITION)
                        conditions.Add(item);
                }
                return conditions;
            }
        }

        public IItem TopItem
        {
            get
            {
                IItem topItem = null;
                foreach (IItem item in _items.Values)
                {
                    if (topItem == null ||
                        (item.Position.Z + item.ItemData.Height) >
                        (topItem.Position.Z + topItem.ItemData.Height))
                    {
                        topItem = item;
                    }
                }
                return topItem;
            }
        }

        public double Height
        {
            get
            {
                double height = _position.Z;
                IItem topItem = TopItem;

                if (topItem != null)
                    height += topItem.ItemData.Height + topItem.Position.Z;

                return height;
            }
        }
    }
}
