using AliasPro.API.Messenger.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Utilities;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class ProfileFriendsComposer : IMessageComposer
    {
        private readonly int _targetId;
        private readonly IList<IMessengerFriend> _love;
        private readonly IList<IMessengerFriend> _happy;
        private readonly IList<IMessengerFriend> _sad;

        public ProfileFriendsComposer(int targetId, ICollection<IMessengerFriend> friends)
        {
            _targetId = targetId;
            _love = new List<IMessengerFriend>();
            _happy = new List<IMessengerFriend>();
            _sad = new List<IMessengerFriend>();

            SetRelationships(friends);
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.ProfileFriendsMessageComposer);
            message.WriteInt(_targetId);

            int total = 0;
            if (_love.Count > 0) total++;
            if (_happy.Count > 0) total++;
            if (_sad.Count > 0) total++;

            message.WriteInt(total);

            if (_love.Count > 0)
            {
                ComposeRelationship(message,
                    _love[Randomness.RandomNumber(_love.Count) - 1]);
            }
            if (_happy.Count > 0)
            {
                ComposeRelationship(message,
                    _love[Randomness.RandomNumber(_happy.Count) - 1]);
            }
            if (_sad.Count > 0)
            {
                ComposeRelationship(message,
                    _love[Randomness.RandomNumber(_sad.Count) - 1]);
            }
            return message;
        }

        private void ComposeRelationship(ServerMessage message, IMessengerFriend friend)
        {
            message.WriteInt(friend.Relation);
            message.WriteInt(_love.Count);
            message.WriteInt((int)friend.Id);
            message.WriteString(friend.Username);
            message.WriteString(friend.Figure);
        }

        private void SetRelationships(ICollection<IMessengerFriend> friends)
        {
            foreach (IMessengerFriend friend in friends)
            {
                switch (friend.Relation)
                {
                    case 1: _love.Add(friend); break;
                    case 2: _love.Add(friend); break;
                    case 3: _love.Add(friend); break;
                }
            }
        }
    }
}
