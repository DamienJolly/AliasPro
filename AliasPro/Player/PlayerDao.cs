using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Player
{
    using Database;
    using Models;
    using Models.Currency;
    using Models.Messenger;

    internal class PlayerDao : BaseDao
    {
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
                }, "SELECT `id`, `credits`, `rank`, `username`, `auth_ticket`, `figure`, `gender`, `motto` FROM `players` WHERE `id` = @0 LIMIT 1;", id);
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
                }, "SELECT `id`, `credits`, `rank`, `username`, `auth_ticket`, `figure`, `gender`, `motto` FROM `players` WHERE `auth_ticket` = @0 LIMIT 1;", sso);
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
                }, "SELECT `id`, `credits`, `rank`, `username`, `auth_ticket`, `figure`, `gender`, `motto` FROM `players` WHERE `username` = @0 LIMIT 1;", username);
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
                }, "SELECT `players`.`id`, `players`.`username`, `players`.`figure`, `players`.`motto`, `messenger_friends`.`relation` " +
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

        internal async Task UpdatePlayerSettings(uint id, IPlayerSettings settings)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `player_settings` SET `navi_x` = @1, `navi_y` = @2, `navi_width` = @3, `navi_height` = @4, `navi_hide_searches` = @5", 
                    id, settings.NaviX, settings.NaviY, settings.NaviWidth, settings.NaviHeight, settings.NaviHideSearches);
            });
        }
        
        internal async Task RemoveAllFriendRequests(uint id)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "DELETE FROM `messenger_requests` WHERE `player_id` = @0;", id);
            });
        }

        internal async Task RemoveFriendRequest(uint id, uint targetId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "DELETE FROM `messenger_requests` WHERE `player_id` = @0 AND `target_id` = @1;", id, targetId);
            });
        }
    }
}
