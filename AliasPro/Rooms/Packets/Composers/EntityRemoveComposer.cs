using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class EntityRemoveComposer : IPacketComposer
    {
        private readonly int _entityId;
        
        public EntityRemoveComposer(int entityId)
        {
            _entityId = entityId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.EntityRemoveMessageComposer);
            message.WriteString(_entityId.ToString());
            return message;
        }
    }
}