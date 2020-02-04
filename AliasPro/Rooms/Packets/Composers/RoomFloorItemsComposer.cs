using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomFloorItemsComposer : IMessageComposer
    {
        private readonly ICollection<IItem> _roomItems;
        private readonly IDictionary<uint, string> _itemOwners;

        public RoomFloorItemsComposer(ICollection<IItem> roomItem, IDictionary<uint, string> itemOwners)
        {
            _roomItems = roomItem;
            _itemOwners = itemOwners;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomFloorItemsMessageComposer);
            message.WriteInt(_itemOwners.Count);
            foreach (var owner in _itemOwners)
            {
                message.WriteInt((int)owner.Key);
                message.WriteString(owner.Value);
            }

            message.WriteInt(_roomItems.Count);
            foreach (IItem item in _roomItems)
            {
                item.ComposeFloorItem(message);
            }
            return message;
        }
    }
}
