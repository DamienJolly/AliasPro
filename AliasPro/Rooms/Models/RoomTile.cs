using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;
using System;
using System.Collections.Generic;

namespace AliasPro.Rooms.Models
{
    internal class RoomTile : IRoomTile
    {
        private readonly IRoom _room;
        private readonly IDictionary<uint, IItem> _items;
        private readonly IDictionary<int, BaseEntity> _entities;

        public IRoomPosition Position { get; private set; }

        internal RoomTile(IRoom room, IRoomPosition position)
        {
            _room = room;
            _items = new Dictionary<uint, IItem>();
            _entities = new Dictionary<int, BaseEntity>();

            Position = position;
        }

        public IRoomPosition PositionInFront(int rotation)
        {
            IRoomPosition position = new RoomPosition(Position.X, Position.Y, Position.Z);

            switch (rotation)
            {
                case 0: position.Y--; break;
                case 1: position.X++; position.Y--; break;
                case 2: position.X++; break;
                case 3: position.X++; position.Y++; break;
                case 4: position.Y++; break;
                case 5: position.X--; position.Y++; break;
                case 6: position.X--; break;
                case 7: position.X--; position.Y--; break;
            }

            return position;
        }

		public bool TilesAdjecent(IRoomPosition targetPos)
		{
			return !(Math.Abs(Position.X - targetPos.X) > 1) && !(Math.Abs(Position.Y - targetPos.Y) > 1);
		}


		public bool IsValidTile(BaseEntity entity, bool final)
        {
            if (_entities.Count > 0)
                return entity == null ? false : _entities.ContainsKey(entity.Id);

            IItem topItem = TopItem;
            if (topItem != null)
            {
				if (entity != null && topItem.ItemData.InteractionType == ItemInteractionType.ONE_WAY_GATE && topItem.InteractingPlayer == entity) return true;

				if (topItem.ItemData.InteractionType == ItemInteractionType.GATE && topItem.Mode >= (topItem.ItemData.Modes - 1)) return true;

				if (topItem.ItemData.InteractionType == ItemInteractionType.CHAIR && final) return true;

				if (topItem.ItemData.InteractionType == ItemInteractionType.BED && final)
				{
					if ((topItem.Rotation % 4) != 0)
					{
						for (int y = topItem.Position.Y; y <= topItem.Position.Y + topItem.ItemData.Width - 1; y++)
						{
							if (topItem.Position.X == Position.X && y == Position.Y)
								return true;
						}
					}
					else
					{
						for (int x = topItem.Position.X; x <= topItem.Position.X + topItem.ItemData.Width - 1; x++)
						{
							if (x == Position.X && topItem.Position.Y == Position.Y)
								return true;
						}
					}
					return false;
				}

				if (topItem.ItemData.CanWalk) return true;

				return false;
            }

            return true;
        }

        public bool CanRoll(IItem item)
        {
            IItem topItem = null;
            foreach (IItem tileItem in _items.Values)
            {
                if (tileItem.ItemData.InteractionType == ItemInteractionType.ROLLER &&
                    item.ItemData.InteractionType == ItemInteractionType.ROLLER)
                    return false;

                if (topItem == null ||
                    (tileItem.Position.Z + tileItem.ItemData.Height) >
                    (topItem.Position.Z + topItem.ItemData.Height))
                {
                    topItem = tileItem;
                }
            }

            if (topItem == item) return true;
            
            if (_entities.Count > 0) return false;

            if (topItem != null)
            {
               if (item.ItemData.InteractionType == ItemInteractionType.ROLLER && 
                    topItem.ItemData.Height > 0) return false;

                return topItem.ItemData.CanStack;
            }

            return true;
        }

        public bool CanStack(IItem item)
        {
            IItem topItem = null;
            foreach (IItem tileItem in _items.Values)
            {
				if (tileItem.ItemData.InteractionType == ItemInteractionType.STACK_TOOL &&
					item.ItemData.InteractionType != ItemInteractionType.STACK_TOOL)
				{
					topItem = tileItem;
					break;
				}

				if (tileItem.ItemData.InteractionType == ItemInteractionType.ROLLER &&
                    item.ItemData.InteractionType == ItemInteractionType.ROLLER)
                    return false;

                if (topItem == null ||
                    (tileItem.Position.Z + tileItem.ItemData.Height) >
                    (topItem.Position.Z + topItem.ItemData.Height))
                {
                    topItem = tileItem;
                }
            }

            if (topItem == item) return true;
            
            if (_entities.Count > 0) return false;

            if (topItem != null)
            {
               if (item.ItemData.InteractionType == ItemInteractionType.ROLLER && 
                    topItem.ItemData.Height > 0) return false;

                return topItem.ItemData.CanStack;
            }

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

        public ICollection<IItem> Items =>
            _items.Values;

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
					if (item.ItemData.InteractionType == ItemInteractionType.STACK_TOOL)
						return item;

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
                double height = Position.Z;
                IItem topItem = TopItem;

				if (topItem != null)
				{
					if (topItem.ItemData.InteractionType == ItemInteractionType.MULTIHEIGHT)
					{
						IList<double> heights = new List<double>();
						foreach (string data in topItem.ItemData.ExtraData.Split(','))
						{
							if (double.TryParse(data, out double heightData))
								heights.Add(heightData);
						}

						if (topItem.Mode <= heights.Count)
							return heights[topItem.Mode] + topItem.Position.Z;
					}

					height += topItem.ItemData.Height + topItem.Position.Z;
				}

                return height;
            }
        }
	}
}
