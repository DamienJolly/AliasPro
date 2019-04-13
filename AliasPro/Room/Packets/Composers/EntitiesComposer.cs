﻿using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Entities;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class EntitiesComposer : IPacketComposer
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

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.EntitiesMessageComposer);
            message.WriteInt(_entities.Count);
            foreach (BaseEntity entity in _entities)
            {
                entity.Compose(message);
            }
            return message;
        }
    }
}
