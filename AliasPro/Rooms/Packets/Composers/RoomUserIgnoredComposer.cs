using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomUserIgnoredComposer : IMessageComposer
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

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomUserIgnoredMessageComposer);
            message.WriteInt(_state);
            message.WriteString(_username);
            return message;
        }
    }
}
