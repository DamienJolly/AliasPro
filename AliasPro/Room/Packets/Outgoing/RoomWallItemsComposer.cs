﻿using System.Collections.Generic;

namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Item.Models;

    public class RoomWallItemsComposer : IPacketComposer
    {
        private readonly ICollection<IItem> _roomItems;
        private readonly IDictionary<uint, string> _itemOwners;

        public RoomWallItemsComposer(ICollection<IItem> roomItem, IDictionary<uint, string> itemOwners)
        {
            _roomItems = roomItem;
            _itemOwners = itemOwners;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomWallItemsMessageComposer);
            message.WriteInt(_itemOwners.Count);
            foreach (var owner in _itemOwners)
            {
                message.WriteInt(owner.Key);
                message.WriteString(owner.Value);
            }
            
            message.WriteInt(_roomItems.Count);
            foreach (IItem item in _roomItems)
            {
                item.ComposeWallItem(message);
            }
            return message;
        }
    }
}
