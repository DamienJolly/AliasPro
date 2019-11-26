using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Items.Models;
using AliasPro.Items.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Items
{
    internal class ItemDao : BaseDao
    {
        public ItemDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

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
                }, "SELECT * FROM `items` WHERE `player_id` = @0 AND `room_id` = '0';", id);
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
                }, "SELECT * FROM `items` WHERE `room_id` = @0;", id);
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

		internal async Task RemoveItemAsync(IItem item)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "DELETE FROM `items` WHERE `id` = @0 AND `player_id` = @1;", item.Id, item.PlayerId);
			});
		}

		internal async Task UpdateRoomItems(ICollection<IItem> items)
        {
            await CreateTransaction(async transaction =>
            {
                foreach (IItem item in items)
                {
                    await Insert(transaction, "UPDATE `items` SET `room_id` = @1, `rot` = @2, `x` = @3, `y` = @4, `z` = @5 WHERE `id` = @0;",
                       item.Id, item.RoomId, item.Rotation, item.Position.X, item.Position.Y, item.Position.Z);
                }
            });
        }

        internal async Task UpdatePlayerItemsAsync(ICollection<IItem> items)
        {
            await CreateTransaction(async transaction =>
            {
                foreach (IItem item in items)
                {
                    await Insert(transaction, "UPDATE `items` SET `room_id` = 0, `extra_data` = @1, `player_id` = @2, `mode` = @3 WHERE `id` = @0;", item.Id, item.ExtraData, item.PlayerId, item.Mode);
                }
            });
        }
    }
}
