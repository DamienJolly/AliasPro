﻿using System.Collections.Generic;
using System.Text;

namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Entities;

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