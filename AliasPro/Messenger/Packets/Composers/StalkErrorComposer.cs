using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Messenger.Packets.Composers
{
    public class StalkErrorComposer : IPacketComposer
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

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.StalkErrorMessageComposer);
            message.WriteInt(_errorCode);
            return message;
        }
    }
}
