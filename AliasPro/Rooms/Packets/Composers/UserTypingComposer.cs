using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class UserTypingComposer : IMessageComposer
    {
        private readonly int _virtualId;
        private readonly bool _typing;

        public UserTypingComposer(int virtualId, bool typing)
        {
            _virtualId = virtualId;
            _typing = typing;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserTypingMessageComposer);
            message.WriteInt(_virtualId);
            message.WriteInt(_typing ? 1 : 0);
            return message;
        }
    }
}
