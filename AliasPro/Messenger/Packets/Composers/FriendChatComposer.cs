using AliasPro.API.Messenger.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Utilities;

namespace AliasPro.Messenger.Packets.Composers
{
    public class FriendChatComposer : IMessageComposer
    {
        private readonly IMessengerMessage _privateMessage;

        public FriendChatComposer(IMessengerMessage privateMessage)
        {
            _privateMessage = privateMessage;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.FriendChatMessageComposer);
            message.WriteInt((int)_privateMessage.TargetId);
            message.WriteString(_privateMessage.Message);
            message.WriteInt((int)UnixTimestamp.Now - 
                _privateMessage.Timestamp);
            return message;
        }
    }
}
