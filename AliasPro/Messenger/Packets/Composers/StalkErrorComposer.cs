using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Messenger.Packets.Composers
{
    public class StalkErrorComposer : IMessageComposer
    {
        public static readonly int NOT_IN_FRIEND_LIST = 0;
        public static readonly int FRIEND_OFFLINE = 1;
        public static readonly int FRIEND_NOT_IN_ROOM = 2;
        public static readonly int FRIEND_BLOCKED_STALKING = 3;

        private readonly int _errorCode;

        public StalkErrorComposer(int errorCode)
        {
            _errorCode = errorCode;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.StalkErrorMessageComposer);
            message.WriteInt(_errorCode);
            return message;
        }
    }
}
