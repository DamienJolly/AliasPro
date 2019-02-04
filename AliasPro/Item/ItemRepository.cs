using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Item
{
    using Models;

    internal class ItemRepository
    {
        private readonly ItemDao _itemDao;
        private IDictionary<uint, IItemData> _itemDatas;

        public ItemRepository(ItemDao itemDao)
        {
            _itemDao = itemDao;

            LoadItemData();
        }

        internal async Task<IDictionary<uint, IItem>> GetItemsForPlayerAsync(uint id) =>
            await _itemDao.GetItemsForPlayerAsync(id, _itemDatas);

        internal async Task<IDictionary<uint, IItem>> GetItemsForRoomAsync(uint id) =>
            await _itemDao.GetItemsForRoomAsync(id, _itemDatas);

        public bool TryGetItemDataById(uint itemId, out IItemData item) =>
            _itemDatas.TryGetValue(itemId, out item);

        private async void LoadItemData() =>
            _itemDatas = await _itemDao.GetItemData();
    }
}
