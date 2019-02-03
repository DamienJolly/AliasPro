using AliasPro.Room.Models.Item;
using System.Collections.Generic;

namespace AliasPro.Room.Gamemap
{
    public class RoomTile
    {
        public IList<IRoomItem> ItemStack { get; }

        internal RoomTile()
        {
            ItemStack = new List<IRoomItem>();
        }

        public void AddItem(IRoomItem item)
        {
            ItemStack.Add(item);
        }

        public void RemoveItem(IRoomItem item)
        {
            ItemStack.Remove(item);
        }

        public bool CanWalkOn()
        {
            IRoomItem topItem = GetTopItem();
            if (topItem != null)
                return topItem.ItemData.CanWalk;
            return true;
        }

        public IRoomItem GetTopItem()
        {
            IRoomItem topItem = null;
            foreach (IRoomItem item in ItemStack)
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
