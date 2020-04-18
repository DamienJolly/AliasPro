using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Items
{
    internal class ItemController : IItemController
    {
		private readonly ItemDao _itemDao;
		private IDictionary<uint, IItemData> _itemDatas;
		private IDictionary<int, ICrackableData> _crackableData;
		private IDictionary<int, ISongData> _songData;

		public ItemController(ItemDao itemDao)
		{
			_itemDao = itemDao;
			_itemDatas = new Dictionary<uint, IItemData>();
			_crackableData = new Dictionary<int, ICrackableData>();
			_songData = new Dictionary<int, ISongData>();

			InitializeItems();
		}

		public async void InitializeItems()
		{
			_itemDatas = await _itemDao.GetItemData();
			_crackableData = await _itemDao.GetCrackableData();
			_songData = await _itemDao.GetSongData();
		}

		public bool TryGetCrackableDataById(int itemId, out ICrackableData crackable) =>
			_crackableData.TryGetValue(itemId, out crackable);

		public bool TryGetSongDataById(int songId, out ISongData song) =>
			_songData.TryGetValue(songId, out song);

		public bool TryGetSongDataByName(string songName, out ISongData song)
		{
			song = null;
			foreach (ISongData songData in _songData.Values)
			{
				if (songData.Code == songName)
				{
					song = songData;
					return true;
				}
			}
			return false;
		}

		public async Task<IDictionary<uint, IItem>> GetItemsForPlayerAsync(uint id) =>
			await _itemDao.GetItemsForPlayerAsync(id, _itemDatas);

		public async Task<IItem> GetPlayerItemByIdAsync(uint itemId) =>
			await _itemDao.GetPlayerItemByIdAsync(itemId, _itemDatas);

		public async Task<IDictionary<uint, IItem>> GetItemsForRoomAsync(uint id) =>
			await _itemDao.GetItemsForRoomAsync(id, _itemDatas);

		public bool TryGetItemDataById(uint itemId, out IItemData item) =>
			_itemDatas.TryGetValue(itemId, out item);

		public bool TryGetItemDataByName(string itemName, out IItemData item)
		{
			item = _itemDatas.Where(x => x.Value.Name == itemName).FirstOrDefault().Value;
			return item != null;
		}

		public async Task<int> AddNewItemAsync(IItem item) =>
			await _itemDao.AddNewItemAsync(item);

		public async Task RemoveItemAsync(IItem item) =>
		   await _itemDao.RemoveItemAsync(item);

		public async Task UpdatePlayerItemsAsync(ICollection<IItem> items) =>
			await _itemDao.UpdatePlayerItemsAsync(items);

		public async Task UpdatePlayerItemAsync(IItem item) =>
			await _itemDao.UpdatePlayerItemAsync(item);
	}
}
