namespace AliasPro.Item
{
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

        public async Task<IDictionary<uint, IItem>> GetItemsForPlayerAsync(uint id) =>
            await _itemRepository.GetItemsForPlayerAsync(id);

        public async Task<IDictionary<uint, IItem>> GetItemsForRoomAsync(uint id) =>
            await _itemRepository.GetItemsForRoomAsync(id);

        public bool TryGetItemDataById(uint itemId, out IItemData item) =>
            _itemRepository.TryGetItemDataById(itemId, out item);

        public async Task<int> AddNewItemAsync(IItem item) =>
            await _itemRepository.AddNewItemAsync(item);
    }

    public interface IItemController
    {
        Task<IDictionary<uint, IItem>> GetItemsForPlayerAsync(uint id);
        Task<IDictionary<uint, IItem>> GetItemsForRoomAsync(uint id);
        bool TryGetItemDataById(uint itemId, out IItemData item);
        Task<int> AddNewItemAsync(IItem item);
    }
}
