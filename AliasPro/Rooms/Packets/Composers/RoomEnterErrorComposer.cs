using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomEnterErrorComposer : IMessageComposer
    {
        public static int ROOM_ERROR_GUESTROOM_FULL = 1;
        public static int ROOM_ERROR_CANT_ENTER = 2;
        public static int ROOM_ERROR_QUE = 3;
        public static int ROOM_ERROR_BANNED = 4;

        public static string ROOM_NEEDS_VIP = "c";
        public static string EVENT_USERS_ONLY = "e1";
        public static string ROOM_LOCKED = "na";
        public static string TO_MANY_SPECTATORS = "spectator_mode_full";

        private readonly int _errorCode;
        private readonly string _queueError;

        public RoomEnterErrorComposer(int errorCode)
        {
            _errorCode = errorCode;
            _queueError = "";
        }

        public RoomEnterErrorComposer(int errorCode, string queueError)
        {
            _errorCode = errorCode;
            _queueError = queueError;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomEnterErrorMessageComposer);
            message.WriteInt(_errorCode);
            message.WriteString(_queueError);
            return message;
        }
    }
}
