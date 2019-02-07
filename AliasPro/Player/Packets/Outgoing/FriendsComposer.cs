using System.Collections.Generic;

namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Messenger;

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
