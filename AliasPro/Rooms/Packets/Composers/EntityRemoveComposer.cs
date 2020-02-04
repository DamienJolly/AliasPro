using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class EntityRemoveComposer : IMessageComposer
    {
        private readonly int _entityId;
        
        public EntityRemoveComposer(int entityId)
        {
            _entityId = entityId;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.EntityRemoveMessageComposer);
            message.WriteString(_entityId.ToString());
            return message;
        }
    }
}