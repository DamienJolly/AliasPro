using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Messenger.Packets.Composers
{
    public class FriendsComposer : IPacketComposer
    {
        private readonly ICollection<IMessengerFriend> _friends;

        public FriendsComposer(ICollection<IMessengerFriend> friends)
        {
            _friends = friends;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.FriendsMessageComposer);
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
