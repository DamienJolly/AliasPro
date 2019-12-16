using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Groups.Models;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Groups.Models;
using AliasPro.Items.Types;
using AliasPro.Players.Types;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Rooms
{
    internal class RoomDao : BaseDao
    {
		public RoomDao(
			IConfigurationController configurationController)
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

		internal async Task CreateRoomPromotionAsync(uint roomId, IRoomPromotion promotion)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "INSERT INTO `room_promotions` (`room_id`, `category_id`, `title`, `description`, `created_timestamp`, `end_timestamp`) VALUES (@0, @1, @2, @3, @4, @5)",
					roomId, promotion.Category, promotion.Title, promotion.Description, promotion.StartTimestamp, promotion.EndTimestamp);
			});
		}

		internal async Task UpdateRoomPromotionAsync(uint roomId, IRoomPromotion promotion)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "UPDATE `room_promotions` SET `category_id` = @1, `title` = @2, `description` = @3, `end_timestamp` = @4 WHERE `room_id` = @0",
					roomId, promotion.Category, promotion.Title, promotion.Description, promotion.EndTimestamp);
			});
		}

		internal async Task CreateRoomWordFilterAsync(string word, uint roomId)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "INSERT INTO `room_wordfilter` (`room_id`, `word`) VALUES (@0, @1)",
					roomId, word);
			});
		}

		internal async Task RemoveRoomWordFilterAsync(string word, uint roomId)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "DELETE FROM `room_wordfilter` WHERE `room_id` = @0 AND `word` = @1",
					roomId, word);
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

		internal async Task<IList<string>> GetWordFilterForRoomAsync(uint roomId)
		{
			IList<string> filters = new List<string>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						string word = reader.ReadData<string>("word");

						if (!filters.Contains(word))
							filters.Add(word);
					}
				}, "SELECT * FROM `room_wordfilter` WHERE `room_id` = @0;",
				roomId);
			});

			return filters;
		}

		internal async Task<IDictionary<int, BaseEntity>> GetBotsForRoomAsync(IRoom room)
		{
			IDictionary<int, BaseEntity> bots = new Dictionary<int, BaseEntity>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						if (!bots.ContainsKey(reader.ReadData<int>("id")))
						{
							BaseEntity botEntity = new BotEntity(
								reader.ReadData<int>("id"),
								(uint)reader.ReadData<int>("player_id"),
								reader.ReadData<string>("username"),
								0,
								reader.ReadData<int>("x"),
								reader.ReadData<int>("y"),
								reader.ReadData<int>("rot"),
								room,
								reader.ReadData<string>("name"),
								reader.ReadData<string>("figure"),
								reader.ReadData<string>("gender") == "m" ? PlayerGender.MALE : PlayerGender.FEMALE,
								reader.ReadData<string>("motto"),
								0,
								reader.ReadData<int>("dance_id"),
								reader.ReadData<bool>("can_walk"));

							bots.Add(botEntity.Id, botEntity);
						}
					}
				}, "SELECT `players`.`username`, `bots`.* FROM `bots` " +
				"INNER JOIN `players` ON `players`.`id` = `bots`.`player_id` WHERE `bots`.`room_id` = @0;",
				room.Id);
			});

			return bots;
		}

		internal async Task<IDictionary<int, BaseEntity>> GetPetsForRoomAsync(IRoom room)
		{
			IDictionary<int, BaseEntity> pets = new Dictionary<int, BaseEntity>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						if (!pets.ContainsKey(reader.ReadData<int>("id")))
						{
							BaseEntity petEntity = new PetEntity(
								reader.ReadData<int>("id"),
								reader.ReadData<int>("type"),
								reader.ReadData<int>("race"),
								reader.ReadData<string>("colour"),
								reader.ReadData<int>("experience"),
								reader.ReadData<int>("happyness"),
								reader.ReadData<int>("energy"),
								reader.ReadData<int>("hunger"),
								reader.ReadData<int>("thirst"),
								reader.ReadData<int>("respect"),
								reader.ReadData<int>("created"),
								(uint)reader.ReadData<int>("player_id"),
								reader.ReadData<string>("username"),
								0,
								reader.ReadData<int>("x"),
								reader.ReadData<int>("y"),
								reader.ReadData<int>("rot"),
								room,
								reader.ReadData<string>("name"));

							pets.Add(reader.ReadData<int>("id"), petEntity);
						}
					}
				}, "SELECT `players`.`username`, `player_pets`.* FROM `player_pets` " +
				"INNER JOIN `players` ON `players`.`id` = `player_pets`.`player_id` WHERE `player_pets`.`room_id` = @0;",
				room.Id);
			});

			return pets;
		}

		internal async Task UpdateBotSettings(BaseEntity entity, uint roomId)
		{
			await CreateTransaction(async transaction =>
			{
				if (!(entity is BotEntity botEntity))
					return;

				await Insert(transaction, "UPDATE `bots` SET `name` = @1, `motto` = @2, `figure` = @3, `gender` = @4, `x` = @5, `y` = @6, " +
					"`z` = @7, `rot` = @8, `room_id` = @9,	`dance_id` = @10, `can_walk` = @11 WHERE `id` = @0;",
				   botEntity.BotId, botEntity.Name, botEntity.Motto, botEntity.Figure, botEntity.Gender == PlayerGender.MALE ? "m" : "f",
				   botEntity.Position.X, botEntity.Position.Y, botEntity.Position.Z, botEntity.BodyRotation, (int)roomId, botEntity.DanceId, botEntity.CanWalk ? "1" : "0");
			});
		}

		internal async Task UpdatePetSettings(BaseEntity entity, uint roomId)
		{
			await CreateTransaction(async transaction =>
			{
				if (!(entity is PetEntity petEntity))
					return;

				await Insert(transaction, "UPDATE `player_pets` SET `name` = @1, `x` = @2, `y` = @3, `z` = @4, `rot` = @5," +
					"`experience` = @6, `happyness` = @7, `energy` = @8, `hunger` = @9, `thirst` = @10, `respect` = @11, `room_id` = @6 WHERE `id` = @0;",
				   petEntity.PetId, petEntity.Name, petEntity.Position.X, petEntity.Position.Y, petEntity.Position.Z, petEntity.BodyRotation, 
				   petEntity.Experience, petEntity.Happyness, petEntity.Energy, petEntity.Hunger, petEntity.Thirst, petEntity.Respect, (int)roomId);
			});
		}

		internal async Task<int> CreateRoomAsync(uint playerId, string name, string description, string modelName, int categoryId, int maxUsers, int tradeType)
        {
            int roomId = -1;
            await CreateTransaction(async transaction =>
            {
                roomId = await Insert(transaction, "INSERT INTO `rooms` (`owner`, `name`, `caption`, `model_name`, `category_id`, `max_users`, `trade_type`) VALUES (@0, @1, @2, @3, @4, @5, @6);",
                    playerId, name, description, modelName, categoryId, maxUsers, tradeType);
            });
            return roomId;
        }

		internal async Task CreateRoomModelAsync(IRoomModel model)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "INSERT INTO `room_models` (`id`, `door_x`, `door_y`, `door_dir`, `heightmap`) VALUES (@0, @1, @2, @3, @4);",
					model.Id, model.DoorX, model.DoorY, model.DoorDir, model.HeightMap);
			});
		}

		internal async Task UpdateRoomModelAsync(IRoomModel model)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "UPDATE `room_models` SET `door_x` = @1, `door_y` = @2, `door_dir` = @3, `heightmap` = @4 WHERE `id` = @0;",
					model.Id, model.DoorX, model.DoorY, model.DoorDir, model.HeightMap);
			});
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
						roomData = new RoomData(reader)
						{
							Group = await GetRoomGroup(reader.ReadData<int>("group_id")),
							Promotion = await GetRoomPromotion((int)reader.ReadData<uint>("id"))
						};
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
						IRoomData data = new RoomData(reader)
						{
							Group = await GetRoomGroup(reader.ReadData<int>("group_id")),
							Promotion = await GetRoomPromotion((int)reader.ReadData<uint>("id"))
						};

						roomData.Add(data);
                    }
                }, "SELECT `rooms`.* , `players`.`username` FROM `rooms` INNER JOIN `players` ON `players`.`id` = `rooms`.`owner` WHERE `rooms`.`owner` = @0;", playerId);
            });

            return roomData;
        }

		internal async Task<IGroup> GetRoomGroup(int groupId)
		{
			IGroup group = null;
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					if (await reader.ReadAsync())
					{
						group = new Group(reader);
					}
				}, "SELECT * FROM `groups` WHERE `id` = @0 LIMIT 1;", groupId);
			});

			return group;
		}

		internal async Task<IRoomPromotion> GetRoomPromotion(int roomId)
		{
			IRoomPromotion promotion = null;
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					if (await reader.ReadAsync())
					{
						if (reader.ReadData<int>("end_timestamp") > (int)UnixTimestamp.Now)
							promotion = new RoomPromotion(reader);
					}
				}, "SELECT * FROM `room_promotions` WHERE `room_id` = @0 ORDER BY `created_timestamp` DESC LIMIT 1;", roomId);
			});

			return promotion;
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

		internal async Task UpdateRoomSettins(IRoom room)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "UPDATE `room_settings` SET `allow_pets` = @1, `allow_pets_eat` = @2, `room_blocking` = @3, `hide_walls` = @4, `wall_thickness` = @5, `floor_thickness` = @6, " +
					"`who_mutes` = @7, `who_kicks` = @8, `who_bans` = @9, `chat_mode` = @10, `chat_size` = @11, `chat_speed` = @12, `chat_distance` = @13, `chat_flood` = @14 WHERE `id` = @0;",
					room.Id, room.Settings.AllowPets, room.Settings.AllowPetsEat, room.Settings.RoomBlocking, room.Settings.HideWalls, room.Settings.WallThickness, room.Settings.FloorThickness, 
					room.Settings.WhoMutes, room.Settings.WhoKicks, room.Settings.WhoBans, 
					room.Settings.ChatMode, room.Settings.ChatSize, room.Settings.ChatSpeed, room.Settings.ChatDistance, room.Settings.ChatFlood);
			});
		}

		internal async Task UpdateRoom(IRoom room)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "UPDATE `rooms` SET `score` = @1, `name` = @2, `caption` = @3, `password` = @4, `max_users` = @5, `trade_type` = @6, `category_id` = @7, `tags` = @8, `model_name` = @9 WHERE `id` = @0;",
					room.Id, room.Score, room.Name, room.Description, room.Password, room.MaxUsers, room.TradeType, room.CategoryId, /*tags*/"", room.ModelName);
			});
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
					await Insert(transaction, "UPDATE `items` SET `room_id` = @1, `rot` = @2, `x` = @3, `y` = @4, `z` = @5, `extra_data` = @6, `mode` = @7, `item_id` = @8 WHERE `id` = @0;",
					   item.Id, item.RoomId, item.Rotation, item.Position.X, item.Position.Y, item.Position.Z, item.ExtraData, item.Mode, item.ItemId);
				}
			});
		}
    }
}
