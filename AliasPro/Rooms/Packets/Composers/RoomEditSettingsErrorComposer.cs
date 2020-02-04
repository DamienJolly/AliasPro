using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomEditSettingsErrorComposer : IMessageComposer
    {
        public static int PASSWORD_REQUIRED = 5;
        public static int ROOM_NAME_MISSING = 7;
        public static int ROOM_NAME_BADWORDS = 8;
        public static int ROOM_DESCRIPTION_BADWORDS = 10;
        public static int ROOM_TAGS_BADWWORDS = 11;
        public static int RESTRICTED_TAGS = 12;
        public static int TAGS_TOO_LONG = 13;

        private readonly uint _roomId;
        private readonly int _errorCode;

        public RoomEditSettingsErrorComposer(uint roomId, int errorCode)
        {
            _roomId = roomId;
            _errorCode = errorCode;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomEditSettingsErrorMessageComposer);
            message.WriteInt((int)_roomId);
            message.WriteInt(_errorCode);
            message.WriteString(string.Empty);
            return message;
        }
    }
}
