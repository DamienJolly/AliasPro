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
    }
}
