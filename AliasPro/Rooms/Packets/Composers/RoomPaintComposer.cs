using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomPaintComposer : IPacketComposer
    {
        private readonly string _type;
        private readonly string _value;

        public RoomPaintComposer(string type, string value)
        {
            _type = type;
            _value = value;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomPaintMessageComposer);
            message.WriteString(_type);
            message.WriteString(_value);
            return message;
        }
    }
}
