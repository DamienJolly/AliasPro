using System.Collections.Generic;

namespace AliasPro.Room.Gamemap
{
    using Item.Models;

    public class RoomTile
    {
        public IList<IItem> ItemStack { get; }

        internal RoomTile()
        {
            ItemStack = new List<IItem>();
        }

        public void AddItem(IItem item)
        {
            ItemStack.Add(item);
        }

        public void RemoveItem(IItem item)
        {
            ItemStack.Remove(item);
        }

        public bool CanWalkOn()
        {
            IItem topItem = GetTopItem();
            if (topItem != null)
                return topItem.ItemData.CanWalk;
            return true;
        }

        public IItem GetTopItem()
        {
            IItem topItem = null;
            foreach (IItem item in ItemStack)
            {
                if (topItem == null)
                {
                    topItem = item;
                    continue;
                }
                if (item.Position.Z + item.ItemData.Height >
                    topItem.Position.Z + topItem.ItemData.Height)
                    topItem = item;
            }
            return topItem;
        }
    }
}
