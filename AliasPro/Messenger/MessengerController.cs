using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Players.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Messenger
{
    internal class MessengerController : IMessengerController
    {
        private readonly MessengerRepository _messengerRepository;

        public MessengerController(MessengerRepository messengerRepository)
        {
            _messengerRepository = messengerRepository;
        }

        public async Task<IDictionary<uint, IMessengerFriend>> GetPlayerFriendsAsync(uint playerId) =>
            await _messengerRepository.GetPlayerFriendsAsync(playerId);
        
        public async Task AddFriendAsync(uint playerId, uint targetId) =>
            await _messengerRepository.AddFriendAsync(playerId, targetId);

        public async Task RemoveFriendAsync(uint playerId, uint targetId) =>
            await _messengerRepository.RemoveFriendAsync(playerId, targetId);


        public async Task<IDictionary<uint, IMessengerRequest>> GetPlayerRequestsAsync(uint id) =>
            await _messengerRepository.GetPlayerRequestsAsync(id);

        public async Task AddRequestAsync(uint playerId, uint targetId) =>
            await _messengerRepository.AddRequestAsync(playerId, targetId);

        public async Task RemoveRequestAsync(uint playerId, uint targetId) =>
            await _messengerRepository.RemoveRequestAsync(playerId, targetId);

        public async Task RemoveAllRequestsAsync(uint playerId) =>
            await _messengerRepository.RemoveAllRequestsAsync(playerId);


        public async Task<ICollection<IMessengerMessage>> GetOfflineMessagesAsync(uint playerId) =>
            await _messengerRepository.GetOfflineMessagesAsync(playerId);

        public async Task AddOfflineMessageAsync(uint playerId, IMessengerMessage privateMessage) =>
           await _messengerRepository.AddOfflineMessageAsync(playerId, privateMessage);


        public async Task UpdateRelationAsync(uint playerId, IMessengerFriend friend) =>
           await _messengerRepository.UpdateRelationAsync(playerId, friend);

        public async Task UpdateStatusAsync(IPlayer player, ICollection<IMessengerFriend> friends) =>
            await _messengerRepository.UpdateStatusAsync(player, friends);
    }
}
