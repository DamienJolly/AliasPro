using System.Collections.Generic;
using System.Text;

namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Entities;

    public class EntityUpdateComposer : IPacketComposer
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

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.EntityUpdateMessageComposer);
            message.WriteInt(_entities.Count);
            foreach (BaseEntity entity in _entities)
            {
                message.WriteInt(entity.Id);
                message.WriteInt(entity.Position.X);
                message.WriteInt(entity.Position.Y);
                message.WriteString(entity.Position.Z.ToString("0.00"));
                message.WriteInt(entity.BodyRotation); //todo: head rotation
                message.WriteInt(entity.BodyRotation);

                StringBuilder statuses = new StringBuilder();
                statuses.Append("/");

                foreach (KeyValuePair<string, string> activeStatus in entity.ActiveStatuses)
                {
                    statuses.Append(activeStatus.Key);

                    if (!string.IsNullOrEmpty(activeStatus.Value))
                    {
                        statuses.Append(" ");
                        statuses.Append(activeStatus.Value);
                    }

                    statuses.Append("/");
                }

                statuses.Append("/");
                message.WriteString(statuses.ToString());
            }
            return message;
        }
    }
}