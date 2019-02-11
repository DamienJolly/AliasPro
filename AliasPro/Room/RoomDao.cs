using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Room
{
    using Database;
    using Item.Models;
    using Models;

    internal class RoomDao : BaseDao
    {
        internal async Task<int> CreateRoom(IRoomData roomData)
        {
            int roomId = -1;
            await CreateTransaction(async transaction =>
            {
                roomId = await Insert(transaction, "INSERT INTO `rooms` (`owner`, `name`, `model_name`) VALUES (@0, @1, @2)", 
                    roomData.OwnerId, roomData.Name, roomData.ModelName);
            });
            return roomId;
        }

        internal async Task<IRoomData> GetRoomData(uint id)
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
                }, "SELECT * FROM `rooms` WHERE `id` = @0 LIMIT 1", id);
            });

            return roomData;
        }

        internal async Task<ICollection<IRoom>> GetAllRoomsById(uint playerId)
        {
            ICollection<IRoom> roomData = new List<IRoom>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        roomData.Add(new Room(new RoomData(reader), null));
                    }
                }, "SELECT * FROM `rooms` WHERE `owner` = @0;", playerId);
            });

            return roomData;
        }

        internal async Task<IRoomSettings> GetRoomSettingsId(uint id)
        {
            IRoomSettings roomSettings = null;
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        roomSettings = new RoomSettings(reader);
                    }
                }, "SELECT * FROM `room_settings` WHERE `id` = @0 LIMIT 1", id);
            });

            return roomSettings;
        }

        internal async Task CreateRoomSettings(uint id)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `room_settings` (`id`) VALUES (@0);", id);
            });
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

        internal async Task UpdateRoomItems(ICollection<IItem> items)
        {
            await CreateTransaction(async transaction =>
            {
                foreach (IItem item in items)
                {
                    await Insert(transaction, "UPDATE `items` SET `room_id` = @1, `rot` = @2, `x` = @3, `y` = @4, `z` = @5, `extra_data` = @6, `mode` = @7 WHERE `id` = @0;",
                       item.Id, item.RoomId, item.Rotation, item.Position.X, item.Position.Y, item.Position.Z, item.ExtraData, item.Mode);
                }
            });
        }
    }
}
