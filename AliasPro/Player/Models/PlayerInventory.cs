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

        public IDictionary<uint, IItem> Items { get; set; }

        internal PlayerInventory(ISession session)
        {
            _session = session;
            Items = new Dictionary<uint, IItem>();
        }

        public Task AddItem(IItem item)
        {
            Items.Add(item.Id, item);
            return Task.CompletedTask;
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
        IDictionary<uint, IItem> Items { get; set; }
        Task AddItem(IItem item);
        Task RemoveItem(IItem item);
        bool TryGetItem(uint id, out IItem item);
    }
}
