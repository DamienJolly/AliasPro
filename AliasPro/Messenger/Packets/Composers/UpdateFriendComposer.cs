using AliasPro.API.Messenger.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Messenger.Packets.Composers
{
    public class UpdateFriendComposer : IMessageComposer
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

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UpdateFriendMessageComposer);
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
                message.WriteInt((int)_friendId);
            }
            return message;
        }
    }
}
