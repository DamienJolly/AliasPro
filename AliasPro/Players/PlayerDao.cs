using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using AliasPro.Players.Models;
using AliasPro.Players.Types;
using AliasPro.Utilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Players
{
    internal class PlayerDao : BaseDao
    {
        public PlayerDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
            : base(logger, configurationController)
        {

        }

        internal async Task<PlayerData> GetPlayerDataAsync(string SSO)
        {
            PlayerData data = null;
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        data = new PlayerData(reader);
                        data.Groups = await GetPlayerGroups((int)data.Id);
                        await ClearSSOAsync(data.Id);
                    }
                }, "SELECT * FROM `players` WHERE `auth_ticket` = @0 LIMIT 1;", SSO);
            });
            return data;
        }

        internal async Task<PlayerData> GetPlayerDataAsync(uint playerId)
        {
            PlayerData data = null;
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        data = new PlayerData(reader);
                        data.Groups = await GetPlayerGroups((int)data.Id);
                    }
                }, "SELECT * FROM `players` WHERE `id` = @0 LIMIT 1;", playerId);
            });
            return data;
        }

        internal async Task<PlayerData> GetPlayerDataByUsernameAsync(string username)
        {
            PlayerData data = null;
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        data = new PlayerData(reader);
                        data.Groups = await GetPlayerGroups((int)data.Id);
                    }
                }, "SELECT * FROM `players` WHERE `username` = @0 LIMIT 1;", username);
            });
            return data;
        }

        internal async Task<int> GetPlayerFriendsAsync(uint playerId)
        {
            int friends = 0;
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        friends = (int)reader.ReadData<long>("count");
                    }
                }, "SELECT COUNT(*) AS `count` FROM `messenger_friends` WHERE `player_id` = @0;", playerId);
            });
            return friends;
        }

        internal async Task ClearSSOAsync(uint playerId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `players` SET `auth_ticket` = '' WHERE `id` = @0;", playerId);
            });
        }

        internal async Task UpdatePlayerAsync(IPlayer player)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `players` SET `is_online` = @1, `motto` = @2, `figure` = @3, `gender` = @4, `last_online` = @5, `home_room` = @6, `group_id` = @7, " +
                    "`respects` = @8, `respects_given` = @9, `respects_recieved` = @10, `login_streak` = @11 WHERE `id` = @0;", 
                    player.Id, player.Online, player.Motto, player.Figure, player.Gender == PlayerGender.MALE ? "m" : "f", (int)UnixTimestamp.Now, player.HomeRoom, player.FavoriteGroup,
                    player.Respects, player.RespectsGiven, player.RespectsRecieved, player.LoginStreak);
            });
        }

        internal async Task<IPlayerSettings> GetPlayerSettingsAsync(uint id)
        {
            IPlayerSettings playerSettings = null;
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        playerSettings = new PlayerSettings(reader);
                    }
                }, "SELECT * FROM `player_settings` WHERE `player_id` = @0 LIMIT 1;", id);
            });

            return playerSettings;
        }

		public async Task<uint> GetPlayerIdByUsername(string username)
		{
			uint userId = 0;
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					if (await reader.ReadAsync())
					{
						userId = reader.ReadData<uint>("id");
					}
				}, "SELECT `id` FROM `players` WHERE `username` = @0 LIMIT 1;", username);
			});
			return userId;
		}

		internal async Task AddPlayerSettingsAsync(uint id)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `player_settings` (`player_id`) VALUES (@0);", id);
            });
        }

        internal async Task UpdatePlayerSettingsAsync(IPlayer player)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `player_settings` SET `navi_x` = @1, `navi_y` = @2, `navi_width` = @3, `navi_height` = @4, `navi_hide_searches` = @5, " +
                    "`ignore_invites` = @6, `camera_follow` = @7, `old_chat` = @8, `volume_system` = @9, `volume_furni` = @10, `volume_trax` = @11 " +
                    "WHERE `player_id` = @0;", player.Id, player.PlayerSettings.NaviX, player.PlayerSettings.NaviY, player.PlayerSettings.NaviWidth, player.PlayerSettings.NaviHeight, 
                    player.PlayerSettings.NaviHideSearches, player.PlayerSettings.IgnoreInvites, player.PlayerSettings.CameraFollow, player.PlayerSettings.OldChat,
                    player.PlayerSettings.VolumeSystem, player.PlayerSettings.VolumeFurni, player.PlayerSettings.VolumeTrax);
            });
        }


        internal async Task<IDictionary<int, IPlayerCurrency>> GetPlayerCurrenciesAsync(uint id)
        {
            IDictionary<int, IPlayerCurrency> currencies = new Dictionary<int, IPlayerCurrency>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IPlayerCurrency currency = new PlayerCurrency(reader);
                        if (!currencies.ContainsKey(currency.Type))
                        {
                            currencies.Add(currency.Type, currency);
                        }
                    }
                }, "SELECT * FROM `player_currencies` WHERE `player_id` = @0;", id);
            });

            return currencies;
        }

        internal async Task<IDictionary<int, string>> GetPlayerIgnoresAsync(uint id)
        {
            IDictionary<int, string> ignores = new Dictionary<int, string>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        int targetId = (int)reader.ReadData<uint>("id");
                        if (!ignores.ContainsKey(targetId))
                            ignores.Add(targetId, reader.ReadData<string>("username"));
                    }
                }, "SELECT `players`.`id`, `players`.`username` FROM `player_ignores` " +
                "INNER JOIN `players` ON `players`.`id` = `player_ignores`.`target_id` WHERE `player_ignores`.`player_id` = @0;", id);
            });
            return ignores;
        }

        internal async Task AddPlayerIgnoreAsync(int playerId, int targetId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `player_ignores` (`player_id`, `target_id`) VALUES (@0, @1);",
                    playerId, targetId);
            });
        }

        internal async Task RemovePlayerIgnoreAsync(int playerId, int targetId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "DELETE FROM `player_ignores` WHERE `player_id` = @0 AND `target_id` = @1;",
                    playerId, targetId);
            });
        }

        internal async Task<IList<int>> GetPlayerRecipesAsync(uint playerId)
        {
            IList<int> recipes = new List<int>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        int recipeId = (int)reader.ReadData<int>("recipe_id");
                        if (!recipes.Contains(recipeId))
                            recipes.Add(recipeId);
                    }
                }, "SELECT `recipe_id` FROM `player_recipes` WHERE `player_id` = @0;", playerId);
            });
            return recipes;
        }

        internal async Task AddPlayerRecipeAsync(int playerId, int recipeId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `player_recipes` (`player_id`, `recipe_id`) VALUES (@0, @1);",
                    playerId, recipeId);
            });
        }

        internal async Task AddPlayerCurrencyAsync(int playerId, IPlayerCurrency currency)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `player_currencies` (`player_id`, `type`, `amount`, `cycles`) VALUES (@0, @1, @2, @3);",
                    playerId, currency.Type, currency.Amount, currency.Cycles);
            });
        }

        internal async Task UpdatePlayerCurrenciesAsync(IPlayer player)
        {
            await CreateTransaction(async transaction =>
            {
                foreach (IPlayerCurrency curreny in player.Currency.Currencies)
                {
                    await Insert(transaction, "UPDATE `player_currencies` SET `amount` = @2, `cycles` = @3 WHERE `player_id` = @0 AND `type` = @1;",
                       player.Id, curreny.Type, curreny.Amount, curreny.Cycles);
                }
            });
        }


        internal async Task<IDictionary<string, IPlayerBadge>> GetPlayerBadgesAsync(uint id)
        {
            IDictionary<string, IPlayerBadge> badges = new Dictionary<string, IPlayerBadge>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IPlayerBadge badge = new PlayerBadge(reader);
                        if (!badges.ContainsKey(badge.Code))
                        {
                            badges.Add(badge.Code, badge);
                        }
                    }
                }, "SELECT `badge_id`, `code`, `slot` FROM `player_badges` WHERE `player_id` = @0;", id);
            });

            return badges;
        }

		internal async Task<IDictionary<int, IPlayerBot>> GetPlayerBotsAsync(uint id)
		{
			IDictionary<int, IPlayerBot> bots = new Dictionary<int, IPlayerBot>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						IPlayerBot bot = new PlayerBot(reader);
						if (!bots.ContainsKey(bot.Id))
						{
							bots.Add(bot.Id, bot);
						}
					}
				}, "SELECT `id`, `name`, `motto`, `gender`, `figure` FROM `bots` WHERE `player_id` = @0 AND `room_id` = '0';", id);
			});

			return bots;
		}

		internal async Task<IDictionary<int, IPlayerPet>> GetPlayerPetsAsync(uint id)
		{
			IDictionary<int, IPlayerPet> pets = new Dictionary<int, IPlayerPet>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						IPlayerPet pet = new PlayerPet(reader);
						if (!pets.ContainsKey(pet.Id))
						{
							pets.Add(pet.Id, pet);
						}
					}
				}, "SELECT * FROM `player_pets` WHERE `player_id` = @0 AND `room_id` = '0';", id);
			});

			return pets;
		}

		internal async Task<IDictionary<int, IPlayerAchievement>> GetPlayerAchievementsAsync(uint id)
		{
			IDictionary<int, IPlayerAchievement> achievements = new Dictionary<int, IPlayerAchievement>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						IPlayerAchievement achievement = new PlayerAchievement(reader);

						if (!achievements.ContainsKey(achievement.Id))
							achievements.Add(achievement.Id, achievement);
					}
				}, "SELECT `id`, `progress` FROM `player_achievements` WHERE `player_id` = @0;", id);
			});

			return achievements;
		}

        internal async Task<IList<IPlayerSanction>> GetPlayerSanctionsAsync(uint id)
        {
            IList<IPlayerSanction> sanctions = new List<IPlayerSanction>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        sanctions.Add(new PlayerSanction(reader));
                    }
                }, "SELECT * FROM `player_sanctions` WHERE `player_id` = @0;", id);
            });
            return sanctions;
        }

        public async Task AddPlayerSanction(uint playerId, IPlayerSanction sanction)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `player_sanctions` (`player_id`, `type`, `reason`, `timestamp`, `expires`, `topic_id`) VALUES (@0, @1, @2, @3, @4, @5);",
                    playerId, sanction.Type.ToString(), sanction.Reason, sanction.StartTime, sanction.ExpireTime, sanction.TopicId);
            });
        }

        internal async Task UpdatePlayerBadgesAsync(IPlayer player)
        {
            await CreateTransaction(async transaction =>
            {
                foreach (IPlayerBadge badge in player.Badge.Badges)
                {
                    await Insert(transaction, "UPDATE `player_badges` SET `slot` = @2 WHERE `code` = @1 AND `player_id` = @0;",
                            player.Id, badge.Code, badge.Slot);
                }
            });
        }

        public async Task<ICollection<IPlayerRoomVisited>> GetPlayerRoomVisitsAsync(uint playerId)
        {
            ICollection<IPlayerRoomVisited> roomsVisisted = new List<IPlayerRoomVisited>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        roomsVisisted.Add(new PlayerRoomVisited(reader));
                    }
                }, "SELECT `player_roomvisits`.*, `rooms`.`name` FROM `player_roomvisits` " +
                "INNER JOIN `rooms` ON `rooms`.`id` = `player_roomvisits`.`room_id` " +
                "WHERE `player_roomvisits`.`player_id` = @0 " +
                "ORDER BY `player_roomvisits`.`entry_timestamp` DESC LIMIT 50;", playerId);
            });
            return roomsVisisted;
        }

		internal async Task<IDictionary<uint, IPlayerData>> GetPlayersByUsernameAsync(string playerName)
		{
			IDictionary<uint, IPlayerData> players = new Dictionary<uint, IPlayerData>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						IPlayerData player = new PlayerData(reader);
                        player.Groups = await GetPlayerGroups((int)player.Id);

						if (!players.ContainsKey(player.Id))
							players.Add(player.Id, player);
					}
				}, "SELECT * FROM `players` WHERE `username` LIKE @0;", 
				"%" + playerName + "%");
			});
			return players;
		}

        internal async Task<IList<int>> GetPlayerGroups(int playerId)
        {
            IList<int> groups = new List<int>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        groups.Add(reader.ReadData<int>("group_id"));
                    }
                }, "SELECT * FROM `group_members` WHERE `player_id` = @0;", playerId);
            });

            return groups;
        }

        internal async Task RemoveFavoriteGroup(int playerId, int groupId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `players` set `group_id` = '0' WHERE `id` = @0 AND `group_id` = @1;",
                    playerId, groupId);
            });
        }

        internal async Task AddPlayerBadge(uint playerId, IPlayerBadge badge)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `player_badges` (`player_id`, `badge_id`, `code`) VALUES (@0, @1, @2);",
                    playerId, badge.BadgeId, badge.Code);
            });
        }

        internal async Task UpdatePlayerBadge(uint playerId, string oldCode, string newCode)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `player_badges` set `code` = @2 WHERE `player_id` = @0 AND `code` = @1;",
                    playerId, oldCode, newCode);
            });
        }

        internal async Task RemovePlayerBadge(uint playerId, string code)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "DELETE FROM `player_badges` WHERE `player_id` = @0 AND `code` = @1;",
                    playerId, code);
            });
        }

        public async Task AddPlayerAchievementAsync(int id, int progress, uint playerId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `player_achievements` (`id`, `player_id`, `progress`) VALUES (@0, @1, @2);",
                    id, playerId, progress);
            });
        }

        public async Task UpdatePlayerAchievementAsync(int id, int progress, uint playerId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `player_achievements` set `progress` = @2 WHERE `id` = @0 AND `player_id` = @1;",
                    id, playerId, progress);
            });
        }
    }
}
