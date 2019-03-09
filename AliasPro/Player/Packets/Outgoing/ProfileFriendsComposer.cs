using System;
using System.Collections.Generic;

namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Messenger;

    public class ProfileFriendsComposer : IPacketComposer
    {
        private readonly uint _targetId;
        private Random _rand;
        private readonly IList<IMessengerFriend> _love;
        private readonly IList<IMessengerFriend> _happy;
        private readonly IList<IMessengerFriend> _sad;

        public ProfileFriendsComposer(uint targetId, ICollection<IMessengerFriend> friends)
        {
            _targetId = targetId;
            _rand = new Random(); //todo: utility
            _love = new List<IMessengerFriend>();
            _happy = new List<IMessengerFriend>();
            _sad = new List<IMessengerFriend>();

            SetRelationships(friends);
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ProfileFriendsMessageComposer);
            message.WriteInt(_targetId);

            int total = 0;
            if (_love.Count > 0) total++;
            if (_happy.Count > 0) total++;
            if (_sad.Count > 0) total++;

            message.WriteInt(total);

            if (_love.Count > 0)
            {
                ComposeRelationship(message,
                    _love[_rand.Next(_love.Count)]);
            }
            if (_happy.Count > 0)
            {
                ComposeRelationship(message,
                    _love[_rand.Next(_happy.Count)]);
            }
            if (_sad.Count > 0)
            {
                ComposeRelationship(message,
                    _love[_rand.Next(_sad.Count)]);
            }
            return message;
        }

        private void ComposeRelationship(ServerPacket message, IMessengerFriend friend)
        {
            message.WriteInt(friend.Relation);
            message.WriteInt(_love.Count);
            message.WriteInt(friend.Id);
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
