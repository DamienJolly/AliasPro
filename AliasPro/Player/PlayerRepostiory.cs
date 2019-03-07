using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Player
{
    using Models;
    using Models.Currency;
    using Models.Messenger;
    using Models.Badge;
    using Item;
    using Packets.Outgoing;

    internal class PlayerRepostiory
    {
        private readonly PlayerDao _playerDao;
        private readonly ItemDao _itemDao;
        private readonly Dictionary<uint, IPlayer> _players;

        public PlayerRepostiory(PlayerDao playerDao, ItemDao itemDao)
        {
            _playerDao = playerDao;
            _itemDao = itemDao;

            _players = new Dictionary<uint, IPlayer>();
        }

        internal async Task UpdateStatus(IPlayer player, ICollection<IMessengerFriend> friends)
        {
            foreach (MessengerFriend friend in friends)
            {
                IPlayer targetPlayer =
                    await GetPlayerById(friend.Id);

                if (targetPlayer == null || 
                    targetPlayer.Session == null || 
                    targetPlayer.Messenger == null) continue;

                if (targetPlayer.Messenger.TryGetFriend(player.Id, out IMessengerFriend targetFriend))
                {
                    targetFriend.Figure = player.Figure;
                    targetFriend.Gender = player.Gender;
                    targetFriend.Username = player.Username;
                    targetFriend.Motto = player.Motto;
                    targetFriend.IsOnline = player.IsOnline;
                    targetFriend.InRoom = player.Session.CurrentRoom != null;

                    await targetPlayer.Session.SendPacketAsync(new UpdateFriendComposer(targetFriend));
                }
            }
        }

        internal async Task<IPlayer> GetPlayerById(uint id)
        {
            if (_players.TryGetValue(id, out IPlayer player)) return player;

            return await _playerDao.GetPlayerById(id);
        }

        internal async Task<IPlayer> GetPlayerByUsername(string username)
        {
            //todo: remove? idk
            foreach (IPlayer player in _players.Values)
            {
                if (player.Username == username)
                    return player;
            }

            return await _playerDao.GetPlayerByUsername(username);
        }

        internal async Task<IPlayer> GetPlayerBySso(string sso)
        {
            IPlayer player = await _playerDao.GetPlayerBySso(sso);
            if(!_players.ContainsKey(player.Id))
            {
                _players.Add(player.Id, player);
            }

            return player;
        }

        public async Task RemovePlayerById(uint playerId)
        {
            IPlayer player = await GetPlayerById(playerId);
            if (player != null)
            {
                player.IsOnline = false;

                await _playerDao.UpdatePlayerById(player);
                await _playerDao.UpdatePlayerSettings(player.Id, player.PlayerSettings);
                await _playerDao.UpdatePlayerCurrencies(player.Id, player.Currency.Currencies);

                if (player.Inventory != null)
                {
                    await _itemDao.UpdatePlayerItems(player.Inventory.Items.Values);
                }

                if (player.Messenger != null)
                {
                    await UpdateStatus(player, player.Messenger.Friends);
                }
            }
            _players.Remove(playerId);
        }

        internal async Task<IDictionary<string, IBadgeData>> GetPlayerBadgesById(uint id) =>
            await _playerDao.GetPlayerBadgesById(id);

        internal async Task UpdatePlayerById(IPlayer player) =>
            await _playerDao.UpdatePlayerById(player);

        internal async Task CreatePlayerSettings(uint id) =>
            await _playerDao.CreatePlayerSettings(id);

        internal async Task CreateFriendRequest(uint playerId, uint targetId) =>
            await _playerDao.CreateFriendRequest(playerId, targetId);

        public async Task CreateFriendShip(uint playerId, uint targetId) =>
            await _playerDao.CreateFriendShip(playerId, targetId);

        public async Task CreateOfflineMessage(uint playerId, IMessengerMessage privateMessage) =>
            await _playerDao.CreateOfflineMessage(playerId, privateMessage);

        internal async Task<IPlayerSettings> GetPlayerSettingsById(uint id) =>
            await _playerDao.GetPlayerSettingsById(id);

        public async Task<IDictionary<int, ICurrencyType>> GetPlayerCurrenciesById(uint id) =>
            await _playerDao.GetPlayerCurrenciesById(id);

        public async Task<IDictionary<uint, IMessengerFriend>> GetPlayerFriendsById(uint id) =>
            await _playerDao.GetPlayerFriendsById(id);

        public async Task<IDictionary<uint, IMessengerRequest>> GetPlayerRequestsById(uint id) =>
            await _playerDao.GetPlayerRequestById(id);

        public async Task<IDictionary<uint, IPlayer>> GetPlayersByUsername(string username) =>
            await _playerDao.GetPlayersByUsername(username);

        public async Task<ICollection<IMessengerMessage>> GetOfflineMessages(uint playerId) =>
            await _playerDao.GetOfflineMessages(playerId);
        
        public async Task RemoveAllFriendRequests(uint playerId) =>
            await _playerDao.RemoveAllFriendRequests(playerId);

        public async Task RemoveFriendRequest(uint playerId, uint targetId) =>
            await _playerDao.RemoveFriendRequest(playerId, targetId);

        public async Task RemoveFriendShip(uint playerId, uint targetId) =>
            await _playerDao.RemoveFriendShip(playerId, targetId);
    }
}
