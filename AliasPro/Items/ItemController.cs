using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Items
{
    internal class ItemController : IItemController
    {
        private readonly ItemRepository _itemRepository;

        public ItemController(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IDictionary<uint, IItem>> GetItemsForPlayerAsync(uint id) =>
            await _itemRepository.GetItemsForPlayerAsync(id);

		public async Task<IItem> GetPlayerItemByIdAsync(uint itemId) =>
			await _itemRepository.GetPlayerItemByIdAsync(itemId);

		public async Task<IDictionary<uint, IItem>> GetItemsForRoomAsync(uint id) =>
            await _itemRepository.GetItemsForRoomAsync(id);

        public bool TryGetItemDataById(uint itemId, out IItemData item) =>
            _itemRepository.TryGetItemDataById(itemId, out item);

        public async Task<int> AddNewItemAsync(IItem item) =>
            await _itemRepository.AddNewItemAsync(item);

		public async Task RemoveItemAsync(IItem item) =>
			await _itemRepository.RemoveItemAsync(item);

		public async Task UpdatePlayerItemsAsync(ICollection<IItem> items) =>
            await _itemRepository.UpdatePlayerItemsAsync(items);
    }
}
