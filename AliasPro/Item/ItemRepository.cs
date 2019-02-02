using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Item
{
    using AliasPro.Player.Models.Inventory;
    using AliasPro.Room.Models.Item;
    using Models;

    internal class ItemRepository
    {
        private readonly ItemDao _itemDao;
        private IDictionary<uint, IItemData> _itemDatas;

        public ItemRepository(ItemDao itemDao)
        {
            _itemDao = itemDao;

            LoadItemData();

            System.Console.WriteLine(_itemDatas.Count);
        }

        internal async Task<IDictionary<uint, IInventoryItem>> GetItemsForPlayerAsync(uint id) =>
            await _itemDao.GetItemsForPlayerAsync(id, _itemDatas);

        internal async Task<IDictionary<uint, IRoomItem>> GetItemsForRoomAsync(uint id) =>
            await _itemDao.GetItemsForRoomAsync(id, _itemDatas);

        public bool TryGetItemDataById(uint itemId, out IItemData item) =>
            _itemDatas.TryGetValue(itemId, out item);

        private async void LoadItemData() =>
            _itemDatas = await _itemDao.GetItemData();
    }
}
