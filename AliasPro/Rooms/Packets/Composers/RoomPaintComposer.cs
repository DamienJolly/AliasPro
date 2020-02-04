using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomPaintComposer : IMessageComposer
    {
        private readonly string _type;
        private readonly string _value;

        public RoomPaintComposer(string type, string value)
        {
            _type = type;
            _value = value;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomPaintMessageComposer);
            message.WriteString(_type);
            message.WriteString(_value);
            return message;
        }
    }
}
