using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Player.Models
{
    using Sessions;
    using Item.Models;
    using Item.Packets.Outgoing;

    internal class PlayerInventory : IPlayerInventory
    {
        private readonly ISession _session;

        public IDictionary<uint, IItem> Items { get; }

        internal PlayerInventory(ISession session, IDictionary<uint, IItem> items)
        {
            _session = session;
            Items = items;
        }

        public async Task AddItem(IItem item)
        {
            Items.Add(item.Id, item);
            await _session.SendPacketAsync(new AddPlayerItemsComposer(item));
        }

        public async Task RemoveItem(IItem item)
        {
            Items.Remove(item.Id);
            await _session.SendPacketAsync(new RemovePlayerItemComposer(item.Id));
        }

        public bool TryGetItem(uint id, out IItem item) => Items.TryGetValue(id, out item);
    }

    public interface IPlayerInventory
    {
        IDictionary<uint, IItem> Items { get; }
        Task AddItem(IItem item);
        Task RemoveItem(IItem item);
        bool TryGetItem(uint id, out IItem item);
    }
}
