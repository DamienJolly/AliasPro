﻿using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Groups.Models;
using AliasPro.API.Players.Models;
using AliasPro.Groups.Models;
using AliasPro.Players.Models;
using AliasPro.Players.Types;
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

        internal async Task<IDictionary<int, ICurrencySetting>> GetCurrencySettings()
        {
            IDictionary<int, ICurrencySetting> settings = new Dictionary<int, ICurrencySetting>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        ICurrencySetting setting = new CurrencySetting(reader);

                        if (!settings.ContainsKey(setting.Id))
                            settings.Add(setting.Id, setting);
                    }
                }, "SELECT * FROM `currency_settings`;");
            });
            return settings;
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
                await Insert(transaction, "UPDATE `players` SET `is_online` = @1, `credits` = @2, `motto` = @3, `figure` = @4, `gender` = @5, `last_online` = @6, `home_room` = @7, `group_id` = @8 WHERE `id` = @0;", 
                    player.Id, player.Online, player.Credits, player.Motto, player.Figure, player.Gender == PlayerGender.MALE ? "m" : "f", player.LastOnline, player.HomeRoom, player.FavoriteGroup);
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
                }, "SELECT `type`, `amount` FROM `player_currencies` WHERE `player_id` = @0;", id);
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

        internal async Task UpdatePlayerCurrenciesAsync(IPlayer player)
        {
            await CreateTransaction(async transaction =>
            {
                foreach (IPlayerCurrency curreny in player.Currency.Currencies)
                {
                    await Insert(transaction, "UPDATE `player_currencies` SET `type` = @1, `amount` = @2 WHERE `player_id` = @0;",
                       player.Id, curreny.Type, curreny.Amount);
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

        internal async Task<IDictionary<int, IGroup>> GetPlayerGroups(int playerId)
        {
            IDictionary<int, IGroup> groups = new Dictionary<int, IGroup>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IGroup group = new Group(reader);

                        if (!groups.ContainsKey(group.Id))
                            groups.Add(group.Id, group);
                    }
                }, "SELECT `groups`.* FROM `group_members` " +
                "INNER JOIN `groups` ON `groups`.`id` = `group_members`.`group_id` " +
                "WHERE `group_members`.`player_id` = @0;", playerId);
            });

            return groups;
        }
    }
}
