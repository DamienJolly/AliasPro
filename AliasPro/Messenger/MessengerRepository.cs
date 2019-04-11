using AliasPro.API.Messenger.Models;
using AliasPro.API.Player.Models;
using AliasPro.Messenger.Packets.Outgoing;
using AliasPro.Player;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Messenger
{
    internal class MessengerRepository
    {
        private readonly MessengerDao _messengerDao;
        private readonly IPlayerController _playerController;

        public MessengerRepository(MessengerDao messengerDao, IPlayerController playerController)
        {
            _messengerDao = messengerDao;
            _playerController = playerController;
        }
        
        public async Task<IDictionary<uint, IMessengerFriend>> GetPlayerFriendsAsync(uint playerId) =>
            await _messengerDao.GetPlayerFriendsAsync(playerId);

        public async Task AddFriendAsync(uint playerId, uint targetId) =>
            await _messengerDao.AddFriendAsync(playerId, targetId);

        public async Task RemoveFriendAsync(uint playerId, uint targetId) =>
            await _messengerDao.RemoveFriendAsync(playerId, targetId);


        public async Task<IDictionary<uint, IMessengerRequest>> GetPlayerRequestsAsync(uint playerId) =>
            await _messengerDao.GetPlayerRequestsAsync(playerId);

        internal async Task AddRequestAsync(uint playerId, uint targetId) =>
            await _messengerDao.AddRequestAsync(playerId, targetId);

        public async Task RemoveRequestAsync(uint playerId, uint targetId) =>
            await _messengerDao.RemoveRequestAsync(playerId, targetId);

        public async Task RemoveAllRequestsAsync(uint playerId) =>
            await _messengerDao.RemoveAllRequestsAsync(playerId);


        public async Task<ICollection<IMessengerMessage>> GetOfflineMessagesAsync(uint playerId) =>
            await _messengerDao.GetOfflineMessagesAsync(playerId);

        public async Task AddOfflineMessageAsync(uint playerId, IMessengerMessage privateMessage) =>
            await _messengerDao.AddOfflineMessageAsync(playerId, privateMessage);
        

        public async Task UpdateRelationAsync(uint playerId, IMessengerFriend friend) =>
           await _messengerDao.UpdateRelationAsync(playerId, friend);

        internal async Task UpdateStatusAsync(IPlayer player, ICollection<IMessengerFriend> friends)
        {
            foreach (IMessengerFriend friend in friends)
            {
                if (!_playerController.TryGetPlayer(friend.Id, out IPlayer targetPlayer))
                    continue;

                if (targetPlayer.Session == null ||  targetPlayer.Messenger == null)
                    continue;

                if (targetPlayer.Messenger.TryGetFriend(player.Id, out IMessengerFriend targetFriend))
                {
                    targetFriend.Figure = player.Figure;
                    targetFriend.Gender = player.Gender;
                    targetFriend.Username = player.Username;
                    targetFriend.Motto = player.Motto;
                    targetFriend.IsOnline = player.Online;
                    targetFriend.InRoom = player.Session.CurrentRoom != null;

                    await targetPlayer.Session.SendPacketAsync(new UpdateFriendComposer(targetFriend));
                }
            }
        }
    }
}
