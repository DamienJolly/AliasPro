﻿namespace AliasPro.Catalog.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class DiscountComposer : IPacketComposer
    {
        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.DiscountMessageComposer);
            message.WriteInt(100);
            message.WriteInt(6);
            message.WriteInt(1);
            message.WriteInt(1);
            message.WriteInt(2);
            {
                message.WriteInt(40);
                message.WriteInt(99);
            }
            return message;
        }
    }
}