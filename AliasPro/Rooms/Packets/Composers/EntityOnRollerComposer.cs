using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class EntityOnRollerComposer : IPacketComposer
    {
        private readonly BaseEntity _entity;
        private readonly IRoomPosition _target;
        private readonly uint _rollerId;

        public EntityOnRollerComposer(BaseEntity entity, IRoomPosition target, uint rollerId)
        {
            _entity = entity;
            _target = target;
            _rollerId = rollerId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.SlideObjectMessageComposer);
            message.WriteInt(_entity.Position.X);
            message.WriteInt(_entity.Position.Y);
            message.WriteInt(_target.X);
            message.WriteInt(_target.Y);
            message.WriteInt(0);
            message.WriteInt(_rollerId);
            message.WriteInt(2);
            message.WriteInt(_entity.Id);
            message.WriteDouble(_entity.Position.Z);
            message.WriteDouble(_target.Z);
            return message;
        }
    }
}
