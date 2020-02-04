using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class EntityOnRollerComposer : IMessageComposer
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

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.SlideObjectMessageComposer);
            message.WriteInt(_entity.Position.X);
            message.WriteInt(_entity.Position.Y);
            message.WriteInt(_target.X);
            message.WriteInt(_target.Y);
            message.WriteInt(0);
            message.WriteInt((int)_rollerId);
            message.WriteInt(2);
            message.WriteInt(_entity.Id);
            message.WriteDouble(_entity.Position.Z);
            message.WriteDouble(_target.Z);
            return message;
        }
    }
}
