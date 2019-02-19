using System.Collections.Generic;

namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Item.Models;

    public class RoomFloorItemsComposer : IPacketComposer
    {
        private readonly ICollection<IItem> _roomItems;
        private readonly IDictionary<uint, string> _itemOwners;

        public RoomFloorItemsComposer(ICollection<IItem> roomItem, IDictionary<uint, string> itemOwners)
        {
            _roomItems = roomItem;
            _itemOwners = itemOwners;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomFloorItemsMessageComposer);
            message.WriteInt(_itemOwners.Count);
            foreach (var owner in _itemOwners)
            {
                message.WriteInt(owner.Key);
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
