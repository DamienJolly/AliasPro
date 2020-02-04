using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class FloorHeightMapComposer : IMessageComposer
    {
        private readonly int _wallHeight;
        private readonly string _map;

        public FloorHeightMapComposer(int wallHeight, string map)
        {
            _wallHeight = wallHeight;
            _map = map;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.FloorHeightMapMessageComposer);
            message.WriteBoolean(true);
            message.WriteInt(_wallHeight);
            message.WriteString(_map);
            return message;
        }
    }
}
