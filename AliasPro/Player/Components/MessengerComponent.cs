using AliasPro.API.Messenger.Models;
using System.Collections.Generic;

namespace AliasPro.Player.Components
{
    public class MessengerComponent
    {
        private readonly IDictionary<uint, IMessengerFriend> _friends;
        private readonly IDictionary<uint, IMessengerRequest> _requests;

        public MessengerComponent(
            IDictionary<uint, IMessengerFriend> friends,
            IDictionary<uint, IMessengerRequest> requests)
        {
            _friends = friends;
            _requests = requests;
        }

        public ICollection<IMessengerFriend> Friends =>
            _friends.Values;

        public bool TryAddFriend(IMessengerFriend friend) =>
           _friends.TryAdd(friend.Id, friend);

        public void RemoveFriend(uint targetId) =>
           _friends.Remove(targetId);

        public bool TryGetFriend(uint id, out IMessengerFriend friend) =>
            _friends.TryGetValue(id, out friend);


        public ICollection<IMessengerRequest> Requests =>
            _requests.Values;

        public bool TryAddRequest(IMessengerRequest request) =>
            _requests.TryAdd(request.Id, request);
        
        public void RemoveRequest(uint targetId) =>
            _requests.Remove(targetId);

        public void RemoveAllRequests() =>
            _requests.Clear();

        public bool TryGetRequest(uint id, out IMessengerRequest request) =>
            _requests.TryGetValue(id, out request);
    }
}
