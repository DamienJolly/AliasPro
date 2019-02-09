using System.Collections.Generic;

namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Item.Models;

    public class RoomWallItemsComposer : IPacketComposer
    {
        private readonly ICollection<IItem> _roomItems;

        public RoomWallItemsComposer(ICollection<IItem> roomItem)
        {
            _roomItems = roomItem;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomWallItemsMessageComposer);
            message.WriteInt(0); //todo: 
            message.WriteInt(_roomItems.Count);
            foreach (IItem item in _roomItems)
            {
                item.ComposeWallItem(message);
            }
            return message;
        }
    }
}
