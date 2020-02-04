using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class EntityUpdateComposer : IMessageComposer
    {
        private readonly ICollection<BaseEntity> _entities;
        
        public EntityUpdateComposer(ICollection<BaseEntity> entities)
        {
            _entities = entities;
        }

        public EntityUpdateComposer(BaseEntity entity)
        {
            _entities = new List<BaseEntity>();
            _entities.Add(entity);
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.EntityUpdateMessageComposer);
            message.WriteInt(_entities.Count);
            foreach (BaseEntity entity in _entities)
            {
                message.WriteInt(entity.Id);
                message.WriteInt(entity.Position.X);
                message.WriteInt(entity.Position.Y);
                message.WriteString(entity.Position.Z.ToString("0.00"));
                message.WriteInt(entity.HeadRotation);
                message.WriteInt(entity.BodyRotation);
                message.WriteString(entity.Actions.StatusToString);
            }
            return message;
        }
    }
}