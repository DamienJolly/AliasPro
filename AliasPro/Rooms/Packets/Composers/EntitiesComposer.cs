using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class EntitiesComposer : IMessageComposer
    {
        private readonly ICollection<BaseEntity> _entities;

        public EntitiesComposer(ICollection<BaseEntity> entities)
        {
            _entities = entities;
        }

        public EntitiesComposer(BaseEntity entity)
        {
            _entities = new List<BaseEntity>();
            _entities.Add(entity);
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.EntitiesMessageComposer);
            message.WriteInt(_entities.Count);
            foreach (BaseEntity entity in _entities)
            {
                entity.Compose(message);
            }
            return message;
        }
    }
}
