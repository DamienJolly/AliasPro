using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomWallItemsComposer : IMessageComposer
    {
        private readonly ICollection<IItem> _roomItems;
        private readonly IDictionary<uint, string> _itemOwners;

        public RoomWallItemsComposer(ICollection<IItem> roomItem, IDictionary<uint, string> itemOwners)
        {
            _roomItems = roomItem;
            _itemOwners = itemOwners;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomWallItemsMessageComposer);
            message.WriteInt(_itemOwners.Count);
            foreach (var owner in _itemOwners)
            {
                message.WriteInt((int)owner.Key);
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
