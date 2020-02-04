using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class UserHandItemComposer : IMessageComposer
    {
        private readonly int _virtualId;
        private readonly int _handItemId;

        public UserHandItemComposer(int virtualId, int handItemId)
        {
            _virtualId = virtualId;
            _handItemId = handItemId;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserHandItemMessageComposer);
            message.WriteInt(_virtualId);
            message.WriteInt(_handItemId);
            return message;
        }
    }
}
