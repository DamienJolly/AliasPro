using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Room.Models.Item
{
    using AliasPro.Item.Models;

    internal class ItemHandler : IDisposable
    {
        internal IDictionary<uint, IItem> RoomItems { get; set; }

        internal ItemHandler(IRoom room)
        {
            RoomItems = new Dictionary<uint, IItem>();
        }

        internal async Task AddItem(IItem item)
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
