using AliasPro.API.Items.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Items
{
    public interface IItemController
    {
        Task<IDictionary<uint, IItem>> GetItemsForPlayerAsync(uint id);
		Task<IItem> GetPlayerItemByIdAsync(uint itemId);
		Task<IDictionary<uint, IItem>> GetItemsForRoomAsync(uint id);
        bool TryGetItemDataById(uint itemId, out IItemData item);
        Task<int> AddNewItemAsync(IItem item);
		Task RemoveItemAsync(IItem item);
		Task UpdatePlayerItemsAsync(ICollection<IItem> items);
		Task UpdatePlayerItemAsync(IItem item);
	}
}
