using AliasPro.API.Messenger.Models;
using AliasPro.API.Players.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Messenger
{
    public interface IMessengerController
    {
        Task<IDictionary<uint, IMessengerFriend>> GetPlayerFriendsAsync(uint id);
        Task AddFriendAsync(uint playerId, uint targetId);
        Task RemoveFriendAsync(uint playerId, uint targetId);

        Task<IDictionary<uint, IMessengerRequest>> GetPlayerRequestsAsync(uint id);
        Task AddRequestAsync(uint playerId, uint targetId);
        Task RemoveRequestAsync(uint playerId, uint targetId);
        Task RemoveAllRequestsAsync(uint playerId);

        Task<ICollection<IMessengerMessage>> GetOfflineMessagesAsync(uint playerId);
        Task AddOfflineMessageAsync(uint playerId, IMessengerMessage privateMessage);

        Task UpdateRelationAsync(uint playerId, IMessengerFriend friend);
        Task UpdateStatusAsync(IPlayer player, ICollection<IMessengerFriend> friends);
    }
}
