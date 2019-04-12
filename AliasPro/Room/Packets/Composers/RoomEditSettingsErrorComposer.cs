using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Room.Packets.Composers
{
    public class RoomEditSettingsErrorComposer : IPacketComposer
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

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomEditSettingsErrorMessageComposer);
            message.WriteInt(_roomId);
            message.WriteInt(_errorCode);
            message.WriteString(string.Empty);
            return message;
        }
    }
}
