using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models.Entities;

namespace AliasPro.Room.Packets.Composers
{
    public class UserActionComposer : IPacketComposer
    {
        private readonly BaseEntity _entity;
        private readonly int _action;

        public UserActionComposer(BaseEntity entity, int action)
        {
            _entity = entity;
            _action = action;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserActionMessageComposer);
            message.WriteInt(_entity.Id);
            message.WriteInt(_action);
            return message;
        }
    }
}
