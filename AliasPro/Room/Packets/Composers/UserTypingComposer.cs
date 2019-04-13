using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class UserTypingComposer : IPacketComposer
    {
        private readonly int _virtualId;
        private readonly bool _typing;

        public UserTypingComposer(int virtualId, bool typing)
        {
            _virtualId = virtualId;
            _typing = typing;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserTypingMessageComposer);
            message.WriteInt(_virtualId);
            message.WriteInt(_typing ? 1 : 0);
            return message;
        }
    }
}
