using System.Collections.Generic;

namespace AliasPro.Room.Gamemap
{
    using Models;
    using Item.Models;

    public class RoomTile
    {
        private readonly IRoom _room;
        private readonly Position _position;
        private readonly IDictionary<uint, IItem> _items;

        internal RoomTile(IRoom room, Position position)
        {
            _room = room;
            _position = position;
            _items = new Dictionary<uint, IItem>();
        }

        public bool IsValidTile(bool final = false)
        {
            IItem topItem = TopItem;
            if (topItem != null)
            {
                if (topItem.ItemData.CanWalk) return true;

                if (topItem.ItemData.CanSit && final) return true;
                
                if (topItem.ItemData.CanLay && final)
                {
                    if ((topItem.Rotation % 4) != 0)
                    {
                        for (int y = topItem.Position.Y; y <= topItem.Position.Y + topItem.ItemData.Width - 1; y++)
                        {
                            if (topItem.Position.X == _position.X && y == _position.Y)
                                return true;
                        }
                    }
                    else
                    {
                        for (int x = topItem.Position.X; x <= topItem.Position.X + topItem.ItemData.Width - 1; x++)
                        {
                            if (x == _position.X && topItem.Position.Y == _position.Y)
                                return true;
                        }
                    }

                    return false;
                }

                return false;
            }

            return true;
        }

        public bool CanStack(IItem item)
        {
            IItem topItem = TopItem;
            //todo: chairs

            if (topItem == item)
                return true;

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
