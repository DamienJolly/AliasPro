using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Item
{
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

        internal async Task<IDictionary<uint, IItem>> GetItemsForPlayerAsync(uint id, IDictionary<uint, IItemData> itemDatas)
        {
            IDictionary<uint, IItem> items = new Dictionary<uint, IItem>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IItem item = new Item(reader);
                        if (!itemDatas.ContainsKey(item.ItemId))
                            continue;

                        item.ItemData = itemDatas[item.ItemId];
                        items.Add(item.Id, item);

                    }
                }, "SELECT `id`, `item_id`, `player_id`, `room_id`, `rot`, `x`, `y`, `z` FROM `items` WHERE `player_id` = @0 AND `room_id` = '0';", id);
            });

            return items;
        }

        internal async Task<IDictionary<uint, IItem>> GetItemsForRoomAsync(uint id, IDictionary<uint, IItemData> itemDatas)
        {
            IDictionary<uint, IItem> items = new Dictionary<uint, IItem>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IItem item = new Item(reader);
                        if (!itemDatas.ContainsKey(item.ItemId))
                            continue;

                        item.ItemData = itemDatas[item.ItemId];
                        items.Add(item.Id, item);

                    }
                }, "SELECT `id`, `item_id`, `player_id`, `room_id`, `rot`, `x`, `y`, `z` FROM `items` WHERE `room_id` = @0;", id);
            });

            return items;
        }

        internal async Task<int> AddNewItemAsync(IItem item)
        {
            int itemId = -1;
            await CreateTransaction(async transaction =>
            {
                itemId = await Insert(transaction, "INSERT INTO `items` (`item_id`, `player_id`) VALUES (@0, @1)", item.ItemId, item.PlayerId);
            });
            return itemId;
        }
    }
}
