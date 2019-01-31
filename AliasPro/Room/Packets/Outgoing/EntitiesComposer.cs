using System.Collections.Generic;

namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Entities;

    public class EntitiesComposer : ServerPacket
    {
        public EntitiesComposer(ICollection<BaseEntity> entities)
            : base(Outgoing.EntitiesMessageComposer)
        {
            WriteInt(entities.Count);
            foreach (BaseEntity entity in entities)
            {
                entity.Compose(this);
            }
        }

        public EntitiesComposer(BaseEntity entity)
            : base(Outgoing.EntitiesMessageComposer)
        {
            WriteInt(1);
            entity.Compose(this);
        }
    }
}
