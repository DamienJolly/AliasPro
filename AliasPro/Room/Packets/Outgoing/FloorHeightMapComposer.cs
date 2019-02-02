namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

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
            message.WriteBoolean(false);
            message.WriteInt(_wallHeight); //todo: wall height
            message.WriteString(_map);
            return message;
        }
    }
}
