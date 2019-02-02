using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Room.Models.Item
{
    internal class ItemHandler : IDisposable
    {
        internal IDictionary<uint, IRoomItem> RoomItems { get; set; }

        internal ItemHandler(IRoom room)
        {
            RoomItems = new Dictionary<uint, IRoomItem>();
        }

        internal async Task AddItem(IRoomItem item)
        {
            RoomItems.Add(item.Id, item);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            RoomItems = null;
        }
    }
}
