using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class AvatarChatComposer : IPacketComposer
    {
        private readonly int _id;
        private readonly string _message;
        private readonly int _expression;
        private readonly int _colour;

        public AvatarChatComposer(int id, string message, int expression, int colour)
        {
            _id = id;
            _message = message;
            _expression = expression;
            _colour = colour;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AvatarChatMessageComposer);
            message.WriteInt(_id);
            message.WriteString(_message);
            message.WriteInt(_expression);
            message.WriteInt(_colour);
            message.WriteInt(0);
            message.WriteInt(-1);
            return message;
        }
    }
}
