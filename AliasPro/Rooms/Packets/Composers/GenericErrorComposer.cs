using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class GenericErrorComposer : IMessageComposer
    {
        public static int AUTHENTICATION_FAILED = -3;
        public static int CONNECTING_TO_THE_SERVER_FAILED = -400;
        public static int ROOM_PASSWORD_INCORRECT = -100002;
        public static int KICKED_OUT_OF_THE_ROOM = 4008;
        public static int NEED_TO_BE_VIP = 4009;
        public static int ROOM_NAME_UNACCEPTABLE = 4010;
        public static int CANNOT_BAN_GROUP_MEMBER = 4011;

        private readonly int _errorCode;

        public GenericErrorComposer(int errorCode)
        {
            _errorCode = errorCode;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.GenericErrorMessageComposer);
            message.WriteInt(_errorCode);
            return message;
        }
    }
}
