using AliasPro.API.Messenger.Models;
using AliasPro.Configuration;
using AliasPro.Database;
using AliasPro.Messenger.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Messenger
{
    internal class MessengerDao : BaseDao
    {
        public MessengerDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

        internal async Task<IDictionary<uint, IMessengerFriend>> GetPlayerFriendsAsync(uint id)
        {
            IDictionary<uint, IMessengerFriend> friends = new Dictionary<uint, IMessengerFriend>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IMessengerFriend friend = new MessengerFriend(reader);
                        if (!friends.TryAdd(friend.Id, friend))
                        {
                            // failed
                        }
                    }
                }, "SELECT `players`.*, `messenger_friends`.* " +
                "FROM `messenger_friends` INNER JOIN `players` ON `players`.`id` = `messenger_friends`.`target_id` WHERE `messenger_friends`.`player_id` = @0;", id);
            });
            return friends;
        }

        internal async Task AddFriendAsync(uint playerId, uint targetId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `messenger_friends` (`player_id`, `target_id`) VALUES (@0, @1);" +
                    "INSERT INTO `messenger_friends` (`player_id`, `target_id`) VALUES (@1, @0);", playerId, targetId);
            });
        }

        internal async Task RemoveFriendAsync(uint playerId, uint targetId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "DELETE FROM `messenger_friends` WHERE (`target_id` = @0 AND `player_id` = @1) OR (`target_id` = @1 AND `player_id` = @0);", playerId, targetId);
            });
        }


        internal async Task<IDictionary<uint, IMessengerRequest>> GetPlayerRequestsAsync(uint id)
        {
            IDictionary<uint, IMessengerRequest> requests = new Dictionary<uint, IMessengerRequest>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        IMessengerRequest request = new MessengerRequest(reader);
                        if (!requests.TryAdd(request.Id, request))
                        {
                            // failed
                        }
                    }
                }, "SELECT `players`.* " +
                "FROM `messenger_requests` INNER JOIN `players` ON `players`.`id` = `messenger_requests`.`target_id` WHERE `messenger_requests`.`player_id` = @0;", id);
            });
            return requests;
        }

        internal async Task AddRequestAsync(uint playerId, uint targetId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `messenger_requests` (`player_id`, `target_id`) VALUES (@0, @1);", playerId, targetId);
            });
        }

        internal async Task RemoveRequestAsync(uint playerId, uint targetId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "DELETE FROM `messenger_requests` WHERE `player_id` = @0 AND `target_id` = @1;", playerId, targetId);
            });
        }

        internal async Task RemoveAllRequestsAsync(uint playerId)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "DELETE FROM `messenger_requests` WHERE `player_id` = @0;", playerId);
            });
        }


        internal async Task<ICollection<IMessengerMessage>> GetOfflineMessagesAsync(uint playerId)
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

        internal async Task AddOfflineMessageAsync(uint playerId, IMessengerMessage privateMessage)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "INSERT INTO `messenger_offline_messages` (`player_id`, `target_id`, `message`, `timestamp`) VALUES(@0, @1, @2, @3);",
                    playerId, privateMessage.TargetId, privateMessage.Message, privateMessage.Timestamp);
            });
        }


        internal async Task UpdateRelationAsync(uint playerId, IMessengerFriend friend)
        {
            await CreateTransaction(async transaction =>
            {
                await Insert(transaction, "UPDATE `messenger_friends` SET `relation` = @2 WHERE `target_id` = @1 AND `player_id` = @0;", playerId, friend.Id, friend.Relation);
            });
        }
    }
}
