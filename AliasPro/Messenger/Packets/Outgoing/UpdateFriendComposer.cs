using AliasPro.API.Messenger.Models;
using AliasPro.Network.Events;
using AliasPro.Network.Protocol;

namespace AliasPro.Messenger.Packets.Outgoing
{
    using Network.Events.Headers;

    public class UpdateFriendComposer : IPacketComposer
    {
        private readonly IMessengerFriend _friend;
        private readonly uint _friendId = 0;

        public UpdateFriendComposer(IMessengerFriend friend)
        {
            _friend = friend;
        }

        public UpdateFriendComposer(uint friendId)
        {
            _friendId = friendId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UpdateFriendMessageComposer);
            message.WriteInt(0);
            message.WriteInt(1);
            if (_friend != null)
            {
                message.WriteInt(0);
                _friend.Compose(message);
            }
            else
            {
                message.WriteInt(-1);
                message.WriteInt(_friendId);
            }
            return message;
        }
    }
}
