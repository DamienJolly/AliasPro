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
                        items.Add(reader.ReadData<uint>("id"), new ItemData(reader));
                    }
                }, "SELECT * FROM `item_data`");
            });

            return items;
        }
    }
}
