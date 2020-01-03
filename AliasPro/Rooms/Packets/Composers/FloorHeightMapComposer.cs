using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class FloorHeightMapComposer : IPacketComposer
    {
        private readonly int _wallHeight;
        private readonly string _map;

        public FloorHeightMapComposer(int wallHeight, string map)
        {
            _wallHeight = wallHeight;
            _map = map;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.FloorHeightMapMessageComposer);
            message.WriteBoolean(true);
            message.WriteInt(_wallHeight);
            message.WriteString(_map);
            return message;
        }
    }
}
