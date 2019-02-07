using System.Collections.Generic;

namespace AliasPro.Player.Models.Messenger
{
    public class MessengerHandler
    {
        private readonly IDictionary<uint, IMessengerFriend> _friends;
        private readonly IDictionary<uint, IMessengerRequest> _requests;

        internal MessengerHandler(
            IDictionary<uint, IMessengerFriend> friends,
            IDictionary<uint, IMessengerRequest> requests)
        {
            _friends = friends;
            _requests = requests;
        }

        public void AddRequest(IMessengerRequest request)
        {
            if (!_requests.ContainsKey(request.Id))
                _requests.Add(request.Id, request);
        }

        public void AddFriend(IMessengerFriend friend)
        {
            if (!_friends.ContainsKey(friend.Id))
                _friends.Add(friend.Id, friend);
        }

        public void RemoveRequest(uint targetId) =>
            _requests.Remove(targetId);

        public void RemoveAllRequests() =>
            _requests.Clear();

        public bool TryGetFriend(uint id, out IMessengerFriend friend) =>
            _friends.TryGetValue(id, out friend);

        public bool TryGetRequest(uint id, out IMessengerRequest request) =>
            _requests.TryGetValue(id, out request);

        public ICollection<IMessengerFriend> Friends =>
            _friends.Values;

        public ICollection<IMessengerRequest> Requests =>
            _requests.Values;
    }
}
