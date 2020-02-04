using AliasPro.API.Messenger.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Messenger.Packets.Composers
{
    public class FriendsComposer : IMessageComposer
    {
        private readonly ICollection<IMessengerFriend> _friends;

        public FriendsComposer(ICollection<IMessengerFriend> friends)
        {
            _friends = friends;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.FriendsMessageComposer);
            message.WriteInt(1);
            message.WriteInt(0);
            message.WriteInt(_friends.Count);
            foreach (IMessengerFriend friend in _friends)
            {
                friend.Compose(message);
            }
            return message;
        }
    }
}
