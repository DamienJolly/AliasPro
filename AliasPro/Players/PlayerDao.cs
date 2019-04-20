using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using AliasPro.Players.Models;
using AliasPro.Players.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Players
{
    internal class PlayerDao : BaseDao
    {
        public PlayerDao(IConfigurationController configurationController)
            : base(configurationController)
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
                        //await ClearSSOAsync(data.Id);
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
                    }
                }, "SELECT * FROM `players` WHERE `id` = @0 LIMIT 1;", playerId);
            });
            return data;
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
                await Insert(transaction, "UPDATE `players` SET `is_online` = @1, `credits` = @2, `motto` = @3, `figure` = @4, `gender` = @5 WHERE `id` = @0;", 
                    player.Id, player.Online, player.Credits, player.Motto, player.Figure, player.Gender == PlayerGender.MALE ? "m" : "f");
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
                }, "SELECT `code`, `slot` FROM `player_badges` WHERE `player_id` = @0;", id);
            });

            return badges;
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
                "ORDER BY `player_roomvisits`.`timestamp` DESC LIMIT 50;", playerId);
            });
            return roomsVisisted;
        }
    }
}
