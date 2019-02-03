using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Room.Models.Item
{
    using AliasPro.Item.Models;
    using AliasPro.Item.Packets.Outgoing;

    internal class ItemHandler : IDisposable
    {
        private readonly IRoom _room;

        internal IDictionary<uint, IItem> RoomItems { get; set; }

        internal ItemHandler(IRoom room)
        {
            _room = room;
            RoomItems = new Dictionary<uint, IItem>();
        }

        internal async Task AddItem(IItem item)
        {
            RoomItems.Add(item.Id, item);
            await _room.SendAsync(new ObjectAddComposer(item));
        }

        internal async Task RemoveItem(IItem item)
        {
            RoomItems.Remove(item.Id);
            //await _room.SendAsync(new ObjectRemoveComposer(item));
        }

        internal async Task UpdateItem(IItem item)
        {
            await _room.SendAsync(new FloorItemUpdateComposer(item));
        }

        public void Dispose()
        {
            RoomItems = null;
        }
    }
}
