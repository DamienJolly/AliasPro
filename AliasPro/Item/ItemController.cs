namespace AliasPro.Item
{
    using AliasPro.Player.Models.Inventory;
    using AliasPro.Room.Models.Item;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class ItemController : IItemController
    {
        private readonly ItemRepository _itemRepository;

        public ItemController(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IDictionary<uint, IInventoryItem>> GetItemsForPlayerAsync(uint id) =>
            await _itemRepository.GetItemsForPlayerAsync(id);

        public async Task<IDictionary<uint, IRoomItem>> GetItemsForRoomAsync(uint id) =>
            await _itemRepository.GetItemsForRoomAsync(id);

        public bool TryGetItemDataById(uint itemId, out IItemData item) =>
            _itemRepository.TryGetItemDataById(itemId, out item);
    }

    public interface IItemController
    {
        Task<IDictionary<uint, IInventoryItem>> GetItemsForPlayerAsync(uint id);
        Task<IDictionary<uint, IRoomItem>> GetItemsForRoomAsync(uint id);
        bool TryGetItemDataById(uint itemId, out IItemData item);
    }
}
