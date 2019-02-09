﻿using System.Collections.Generic;

namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Item.Models;

    public class RoomFloorItemsComposer : IPacketComposer
    {
        private readonly ICollection<IItem> _roomItems;

        public RoomFloorItemsComposer(ICollection<IItem> roomItem)
        {
            _roomItems = roomItem;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomFloorItemsMessageComposer);
            message.WriteInt(0); //todo: 
            message.WriteInt(_roomItems.Count);
            foreach (IItem item in _roomItems)
            {
                item.ComposeFloorItem(message);
            }
            return message;
        }
    }
}
