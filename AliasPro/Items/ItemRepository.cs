using AliasPro.API.Items.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Items
{
    internal class ItemRepository
    {
        private readonly ItemDao _itemDao;
        private IDictionary<uint, IItemData> _itemDatas;

        public ItemRepository(ItemDao itemDao)
        {
            _itemDao = itemDao;
            _itemDatas = new Dictionary<uint, IItemData>();
            
            InitializeItem();
        }

        private async void InitializeItem()
        {
            _itemDatas = await _itemDao.GetItemData();
        }

        internal async Task<IDictionary<uint, IItem>> GetItemsForPlayerAsync(uint id) =>
            await _itemDao.GetItemsForPlayerAsync(id, _itemDatas);

		internal async Task<IItem> GetPlayerItemByIdAsync(uint itemId) =>
			await _itemDao.GetPlayerItemByIdAsync(itemId, _itemDatas);

		internal async Task<IDictionary<uint, IItem>> GetItemsForRoomAsync(uint id) =>
            await _itemDao.GetItemsForRoomAsync(id, _itemDatas);

        public bool TryGetItemDataById(uint itemId, out IItemData item) =>
            _itemDatas.TryGetValue(itemId, out item);

        public async Task<int> AddNewItemAsync(IItem item) =>
            await _itemDao.AddNewItemAsync(item);

		public async Task RemoveItemAsync(IItem item) =>
		   await _itemDao.RemoveItemAsync(item);

		public async Task UpdatePlayerItemsAsync(ICollection<IItem> items) =>
            await _itemDao.UpdatePlayerItemsAsync(items);
    }
}
