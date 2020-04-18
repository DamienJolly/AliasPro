using AliasPro.API.Items.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Items
{
    public interface IItemController
    {
		bool TryGetCrackableDataById(int itemId, out ICrackableData crackable);
		bool TryGetSongDataById(int songId, out ISongData song);
		bool TryGetSongDataByName(string songName, out ISongData song);
		Task<IDictionary<uint, IItem>> GetItemsForPlayerAsync(uint id);
		Task<IItem> GetPlayerItemByIdAsync(uint itemId);
		Task<IDictionary<uint, IItem>> GetItemsForRoomAsync(uint id);
        bool TryGetItemDataById(uint itemId, out IItemData item);
        bool TryGetItemDataByName(string itemName, out IItemData item);
        Task<int> AddNewItemAsync(IItem item);
		Task RemoveItemAsync(IItem item);
		Task UpdatePlayerItemsAsync(ICollection<IItem> items);
		Task UpdatePlayerItemAsync(IItem item);
		void InitializeItems();
	}
}
