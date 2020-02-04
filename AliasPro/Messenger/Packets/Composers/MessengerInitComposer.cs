using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Messenger.Packets.Composers
{
    public class MessengerInitComposer : IMessageComposer
    {
        private readonly int _maxFriends;

        public MessengerInitComposer(int maxFriends)
        {
            _maxFriends = maxFriends;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.MessengerInitMessageComposer);
            message.WriteInt(_maxFriends);
            message.WriteInt(300);
            message.WriteInt(800);
            message.WriteInt(0);
            message.WriteBoolean(true);
            return message;
        }
    }
}
