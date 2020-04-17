using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Items.Models;
using AliasPro.Items.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Items
{
    internal class ItemDao : BaseDao
    {
        public ItemDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
            : base(logger, configurationController)
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
                        items.TryAdd(item.Id, item);
                    }
                }, "SELECT * FROM `item_data`");
            });

            return items;
        }

        internal async Task<IDictionary<int, ICrackableData>> GetCrackableData()
        {
            IDictionary<int, ICrackableData> crackables = new Dictionary<int, ICrackableData>();

            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        ICrackableData crackable = new CrackableData(reader);
                        crackables.TryAdd(crackable.ItemId, crackable);
                    }
                }, "SELECT * FROM `items_crackable`");
            });

            return crackables;
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
                }, "SELECT `items`.*, `players`.`username` FROM `items` INNER JOIN `players` ON `players`.`id` = `items`.`player_id` WHERE `items`.`player_id` = @0 AND `items`.`room_id` = '0';", id);
            });

            return items;
        }

		internal async Task<IItem> GetPlayerItemByIdAsync(uint itemId, IDictionary<uint, IItemData> itemDatas)
		{
			IItem item = null;
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					if (await reader.ReadAsync())
					{
						IItem itemToGo = new Item(reader);
						if (itemDatas.ContainsKey(itemToGo.ItemId))
						{
							itemToGo.ItemData = itemDatas[itemToGo.ItemId];
							item = itemToGo;
						}

					}
				}, "SELECT `items`.*, `players`.`username` FROM `items` INNER JOIN `players` ON `players`.`id` = `items`.`player_id` WHERE `items`.`id` = @0;", itemId);
            });

			return item;
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
                }, "SELECT `items`.*, `players`.`username` FROM `items` INNER JOIN `players` ON `players`.`id` = `items`.`player_id` WHERE `items`.`room_id` = @0;", id);
            });

            return items;
        }

        internal async Task<int> AddNewItemAsync(IItem item)
        {
            int itemId = -1;
            await CreateTransaction(async transaction =>
            {
                itemId = await Insert(transaction, "INSERT INTO `items` (`item_id`, `player_id`, `extra_data`) VALUES (@0, @1, @2)", item.ItemId, item.PlayerId, item.ExtraData);
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

        internal async Task UpdatePlayerItemsAsync(ICollection<IItem> items)
        {
            await CreateTransaction(async transaction =>
            {
                foreach (IItem item in items)
                {
                    await Insert(transaction, "UPDATE `items` SET `room_id` = 0, `extra_data` = @1, `player_id` = @2, `item_id` = @3 WHERE `id` = @0;", item.Id, item.ExtraData, item.PlayerId, item.ItemId);
                }
            });
        }

        internal async Task UpdatePlayerItemAsync(IItem item)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `items` SET `room_id` = 0, `extra_data` = @1, `player_id` = @2, `item_id` = @3 WHERE `id` = @0;", 
                    item.Id, item.ExtraData, item.PlayerId,item.ItemId);
            });
        }
    }
}
