using AliasPro.Network.Events;
using AliasPro.Network.Protocol;

namespace AliasPro.Messenger.Packets.Outgoing
{
    using Network.Events.Headers;

    public class RoomInviteErrorComposer : IPacketComposer
    {
        public static readonly int FRIEND_MUTED = 3;
        public static readonly int YOU_ARE_MUTED = 4;
        public static readonly int FRIEND_NOT_ONLINE = 5; //Offline Messages?
        public static readonly int NO_FRIENDS = 6;
        public static readonly int FRIEND_BUSY = 7;
        public static readonly int OFFLINE_FAILED = 10;

        private readonly int _errorCode;
        private readonly uint _targetId;

        public RoomInviteErrorComposer(int errorCode, uint targetId)
        {
            _errorCode = errorCode;
            _targetId = targetId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomInviteErrorMessageComposer);
            message.WriteInt(_errorCode);
            message.WriteInt(_targetId);
            message.WriteString("");
            return message;
        }
    }
}
