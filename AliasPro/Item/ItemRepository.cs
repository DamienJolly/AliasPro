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

            System.Console.WriteLine(_itemDatas.Count);
        }

        private async void LoadItemData() =>
            _itemDatas = await _itemDao.GetItemData();
    }
}
