using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomUserIgnoredComposer : IPacketComposer
    {
        public static int IGNORED = 1;
        public static int MUTED = 2;
        public static int UNIGNORED = 3;

        private readonly string _username;
        private readonly int _state;

        public RoomUserIgnoredComposer(string username, int state)
        {
            _username = username;
            _state = state;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomUserIgnoredMessageComposer);
            message.WriteInt(_state);
            message.WriteString(_username);
            return message;
        }
    }
}
