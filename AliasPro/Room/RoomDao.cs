using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Room
{
    using AliasPro.Room.Models.Item;
    using Database;
    using Models;

    internal class RoomDao : BaseDao
    {
        internal async Task<IRoomData> GetRoomData(int id)
        {
            IRoomData roomData = null;
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        roomData = new RoomData(reader);
                    }
                }, "SELECT `id`, `score`, `owner`, `name`, `password`, `model_name` FROM `rooms` WHERE `id` = @0 LIMIT 1", id);
            });

            return roomData;
        }

        internal async Task<IEnumerable<IRoomModel>> GetRoomModels()
        {
            IList<IRoomModel> roomModels = new List<IRoomModel>();

            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        roomModels.Add(new RoomModel(reader));
                    }
                }, "SELECT `id`, `door_x`, `door_y`, `door_dir`, `heightmap` FROM `room_models`");
            });

            return roomModels;
        }

        internal async Task<IDictionary<uint, IRoomItem>> GetRoomItems(int id)
        {
            IDictionary<uint, IRoomItem> items = new Dictionary<uint, IRoomItem>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IRoomItem item = new RoomItem(reader);
                        items.Add(item.Id, item);

                    }
                }, "SELECT `id`, `item_id`, `rot`, `x`, `y`, `z` FROM `room_items` WHERE `player_id` = @0 LIMIT 1;", id);
            });

            return items;
        }
    }
}
