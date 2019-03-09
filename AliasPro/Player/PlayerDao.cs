﻿using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Player
{
    using Configuration;
    using Database;
    using Models;
    using Models.Currency;
    using Models.Messenger;
    using Models.Badge;

    internal class PlayerDao : BaseDao
    {
        public PlayerDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

        internal async Task CreatePlayerSettings(uint id)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `player_settings` (`player_id`) VALUES (@0);", id);
            });
        }

        internal async Task CreateFriendRequest(uint playerId, uint targetId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `messenger_requests` (`player_id`, `target_id`) VALUES (@0, @1);", playerId, targetId);
            });
        }

        internal async Task CreateFriendShip(uint playerId, uint targetId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `messenger_friends` (`player_id`, `target_id`) VALUES (@0, @1);" +
                    "INSERT INTO `messenger_friends` (`player_id`, `target_id`) VALUES (@1, @0);", playerId, targetId);
            });
        }

        internal async Task UpdatePlayerById(IPlayer player)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `players` set `credits` = @1, `is_online` = @2, `username` = @3, `figure` = @4 WHERE `id` = @0;", 
                    player.Id, player.Credits, player.IsOnline, player.Username, player.Figure);
            });
        }

        internal async Task CreateOfflineMessage(uint playerId, IMessengerMessage privateMessage)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `messenger_offline_messages` (`player_id`, `target_id`, `message`, `timestamp`) VALUES(@0, @1, @2, @3);", 
                    playerId, privateMessage.TargetId, privateMessage.Message, privateMessage.Timestamp);
            });
        }

        internal async Task<IPlayer> GetPlayerById(uint id)
        {
            IPlayer player = null;
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        player = new Player(reader);
                    }
                }, "SELECT `id`, `credits`, `rank`, `username`, `auth_ticket`, `figure`, `gender`, `motto`, `is_online` FROM `players` WHERE `id` = @0 LIMIT 1;", id);
            });
            return player;
        }

        internal async Task<IPlayer> GetPlayerBySso(string sso)
        {
            IPlayer player = null;
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        player = new Player(reader);
                    }
                }, "SELECT `id`, `credits`, `rank`, `username`, `auth_ticket`, `figure`, `gender`, `motto`, `is_online` FROM `players` WHERE `auth_ticket` = @0 LIMIT 1;", sso);
            });
            return player;
        }

        internal async Task<IPlayer> GetPlayerByUsername(string username)
        {
            IPlayer player = null;
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    if (await reader.ReadAsync())
                    {
                        player = new Player(reader);
                    }
                }, "SELECT `id`, `credits`, `rank`, `username`, `auth_ticket`, `figure`, `gender`, `motto`, `is_online` FROM `players` WHERE `username` = @0 LIMIT 1;", username);
            });
            return player;
        }

        internal async Task<IPlayerSettings> GetPlayerSettingsById(uint id)
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
                }, "SELECT `navi_x`, `navi_y`, `navi_width`, `navi_height`, `navi_hide_searches` FROM `player_settings` WHERE `player_id` = @0 LIMIT 1;", id);
            });

            return playerSettings;
        }

        internal async Task<IDictionary<int, ICurrencyType>> GetPlayerCurrenciesById(uint id)
        {
            IDictionary<int, ICurrencyType> currencies = new Dictionary<int, ICurrencyType>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        ICurrencyType currency = new CurrencyType(reader);
                        if (!currencies.ContainsKey(currency.Type))
                        {
                            currencies.Add(currency.Type, currency);
                        }
                    }
                }, "SELECT `type`, `amount` FROM `player_currencies` WHERE `player_id` = @0;", id);
            });

            return currencies;
        }

        internal async Task<IDictionary<string, IBadgeData>> GetPlayerBadgesById(uint id)
        {
            IDictionary<string, IBadgeData> badges = new Dictionary<string, IBadgeData>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IBadgeData badge = new BadgeData(reader);
                        if (!badges.ContainsKey(badge.Code))
                        {
                            badges.Add(badge.Code, badge);
                        }
                    }
                }, "SELECT `code`, `slot` FROM `player_badges` WHERE `player_id` = @0;", id);
            });

            return badges;
        }

        internal async Task<IDictionary<uint, IMessengerFriend>> GetPlayerFriendsById(uint id)
        {
            IDictionary<uint, IMessengerFriend> friends = new Dictionary<uint, IMessengerFriend>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IMessengerFriend friend = new MessengerFriend(reader);
                        if (!friends.ContainsKey(friend.Id))
                        {
                            friends.Add(friend.Id, friend);
                        }
                    }
                }, "SELECT `players`.`id`, `players`.`username`, `players`.`figure`, `players`.`gender`, `players`.`motto`, `players`.`is_online`, `messenger_friends`.`relation` " +
                "FROM `messenger_friends` INNER JOIN `players` ON `players`.`id` = `messenger_friends`.`target_id` WHERE `messenger_friends`.`player_id` = @0;", id);
            });
            return friends;
        }

        internal async Task<IDictionary<uint, IMessengerRequest>> GetPlayerRequestById(uint id)
        {
            IDictionary<uint, IMessengerRequest> requests = new Dictionary<uint, IMessengerRequest>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IMessengerRequest request = new MessengerRequest(reader);
                        if (!requests.ContainsKey(request.Id))
                        {
                            requests.Add(request.Id, request);
                        }
                    }
                }, "SELECT `players`.`id`, `players`.`username`, `players`.`figure`, `players`.`motto` " +
                "FROM `messenger_requests` INNER JOIN `players` ON `players`.`id` = `messenger_requests`.`target_id` WHERE `messenger_requests`.`player_id` = @0;", id);
            });
            return requests;
        }

        internal async Task<IDictionary<uint, IPlayer>> GetPlayersByUsername(string username)
        {
            IDictionary<uint, IPlayer> players = new Dictionary<uint, IPlayer>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IPlayer player = new Player(reader);
                        if (!players.ContainsKey(player.Id))
                        {
                            players.Add(player.Id, player);
                        }
                    }
                }, "SELECT `id`, `credits`, `rank`, `username`, `auth_ticket`, `figure`, `gender`, `motto`, `is_online` FROM `players` WHERE `username` LIKE @0 LIMIT 1;", "%" + username + "%");
            });
            return players;
        }

        internal async Task<ICollection<IMessengerMessage>> GetOfflineMessages(uint playerId)
        {
            ICollection<IMessengerMessage> messages = new List<IMessengerMessage>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IMessengerMessage message = new MessengerMessage(reader);
                        messages.Add(message);
                    }
                }, "SELECT `target_id`, `message`, `timestamp` FROM `messenger_offline_messages` WHERE `player_id` = @0;", playerId);
            });

            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "DELETE FROM `messenger_offline_messages` WHERE `player_id` = @0;", playerId);
            });

            return messages;
        }

        internal async Task UpdatePlayerSettings(uint id, IPlayerSettings settings)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `player_settings` SET `navi_x` = @1, `navi_y` = @2, `navi_width` = @3, `navi_height` = @4, `navi_hide_searches` = @5 " +
                    "WHERE `player_id` = @0;", id, settings.NaviX, settings.NaviY, settings.NaviWidth, settings.NaviHeight, settings.NaviHideSearches);
            });
        }

        internal async Task UpdatePlayerCurrencies(uint id, ICollection<ICurrencyType> currencies)
        {
            await CreateTransaction(async transaction =>
            {
                foreach (ICurrencyType curreny in currencies)
                {
                    await Insert(transaction, "UPDATE `player_currencies` SET `type` = @1, `amount` = @2 WHERE `player_id` = @0;",
                       id, curreny.Type, curreny.Amount);
                }
            });
        }

        internal async Task RemoveAllFriendRequests(uint playerId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "DELETE FROM `messenger_requests` WHERE `player_id` = @0;", playerId);
            });
        }

        internal async Task RemoveFriendRequest(uint playerId, uint targetId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "DELETE FROM `messenger_requests` WHERE `player_id` = @0 AND `target_id` = @1;", playerId, targetId);
            });
        }

        internal async Task RemoveFriendShip(uint playerId, uint targetId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "DELETE FROM `messenger_friends` WHERE (`target_id` = @0 AND `player_id` = @1) OR (`target_id` = @1 AND `player_id` = @0);", playerId, targetId);
            });
        }

        internal async Task ResetPlayerWearableBadges(uint playerId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `player_badges` SET `slot` = '0' WHERE `player_id` = @0;", playerId);
            });
        }

        internal async Task UpdatePlayerWearableBadge(uint playerId, string code, int slot)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `player_badges` SET `slot` = @2 WHERE `code` = @1 AND `player_id` = @0;", playerId, code, slot);
            });
        }

        internal async Task UpdateFriendRelation(uint playerId, IMessengerFriend friend)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `messenger_friends` SET `relation` = @2 WHERE `target_id` = @1 AND `player_id` = @0;", playerId, friend.Id, friend.Relation);
            });
        }
    }
}
