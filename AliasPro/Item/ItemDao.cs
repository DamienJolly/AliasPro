using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Item
{
    using AliasPro.Player.Models.Inventory;
    using AliasPro.Room.Models.Item;
    using Database;
    using Models;

    internal class ItemDao : BaseDao
    {
        internal async Task<IDictionary<uint, IItemData>> GetItemData()
        {
            IDictionary<uint, IItemData> items = new Dictionary<uint, IItemData>();

            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        ItemData item = new ItemData(reader);
                        items.Add(item.Id, item);
                    }
                }, "SELECT * FROM `item_data`");
            });

            return items;
        }

        internal async Task<IDictionary<uint, IInventoryItem>> GetItemsForPlayerAsync(uint id, IDictionary<uint, IItemData> itemDatas)
        {
            IDictionary<uint, IInventoryItem> items = new Dictionary<uint, IInventoryItem>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IInventoryItem item = new InventoryItem(reader);
                        if (!itemDatas.ContainsKey(item.ItemId))
                            continue;

                        item.ItemData = itemDatas[item.ItemId];
                        items.Add(item.Id, item);

                    }
                }, "SELECT `id`, `item_id` FROM `player_items` WHERE `player_id` = @0 LIMIT 1;", id);
            });

            return items;
        }

        internal async Task<IDictionary<uint, IRoomItem>> GetItemsForRoomAsync(uint id, IDictionary<uint, IItemData> itemDatas)
        {
            IDictionary<uint, IRoomItem> items = new Dictionary<uint, IRoomItem>();
            return items;
        }
    }
}
