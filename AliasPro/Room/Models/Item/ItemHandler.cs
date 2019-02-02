using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Room.Models.Item
{
    public class ItemHandler : IDisposable
    {
        public IDictionary<uint, IRoomItem> RoomItems { get; set; }

        public ItemHandler(IRoom room)
        {
            RoomItems = new Dictionary<uint, IRoomItem>();
        }

        public async Task AddItem(IRoomItem item)
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
