using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Rooms
{
    internal class RoomDao : BaseDao
    {
        public RoomDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

        internal async Task CreateRoomRights(uint roomId, uint playerId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `room_rights` (`room_id`, `player_id`) VALUES (@0, @1)",
                    roomId, playerId);
            });
        }

        internal async Task RemoveRoomRights(uint roomId, uint playerId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "DELETE FROM `room_rights` WHERE `room_id` = @0 AND `player_id` = @1",
                    roomId, playerId);
            });
        }

        internal async Task<IDictionary<uint, string>> GetRightsForRoom(uint roomId)
        {
            IDictionary<uint, string> rights = new Dictionary<uint, string>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        if(!rights.ContainsKey(reader.ReadData<uint>("id")))
                        {
                            rights.Add(reader.ReadData<uint>("id"),
                                reader.ReadData<string>("username"));
                        }
                    }
                }, "SELECT `players`.`id`, `players`.`username` FROM `room_rights` " +
                "INNER JOIN `players` ON `players`.`id` = `room_rights`.`player_id` WHERE `room_rights`.`room_id` = @0;",
                roomId);
            });

            return rights;
        }

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
                }, "SELECT `rooms`.* , `players`.`username` FROM `rooms` INNER JOIN `players` ON `players`.`id` = `rooms`.`owner` WHERE `rooms`.`id` = @0 LIMIT 1", id);
            });

            return roomData;
        }

        internal async Task<ICollection<IRoomData>> GetAllRoomDataById(uint playerId)
        {
            ICollection<IRoomData> roomData = new List<IRoomData>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        roomData.Add(new RoomData(reader));
                    }
                }, "SELECT `rooms`.* , `players`.`username` FROM `rooms` INNER JOIN `players` ON `players`.`id` = `rooms`.`owner` WHERE `rooms`.`owner` = @0;", playerId);
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
