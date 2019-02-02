namespace AliasPro.Room.Packets.Outgoing
{
    using AliasPro.Room.Models.Item;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using System.Collections.Generic;

    public class RoomFloorItemsComposer : IPacketComposer
    {
        private readonly ICollection<IRoomItem> _roomItems;

        public RoomFloorItemsComposer(ICollection<IRoomItem> roomItem)
        {
            _roomItems = roomItem;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomFloorItemsMessageComposer);
            message.WriteInt(0); //todo: 
            message.WriteInt(_roomItems.Count);
            foreach (IRoomItem item in _roomItems)
            {
                message.WriteInt(item.Id);
                message.WriteInt(item.ItemData.SpriteId);
                message.WriteInt(item.Position.X);
                message.WriteInt(item.Position.Y);
                message.WriteInt(item.Rotation);
                message.WriteString(item.Position.Z.ToString());
                message.WriteString(item.ItemData.Height.ToString());
                message.WriteInt(1);
                message.WriteInt(0);
                message.WriteString(item.Mode.ToString());
                message.WriteInt(-1); // item Rent time
                message.WriteInt((item.ItemData.Modes > 1) ? 1 : 0);
                message.WriteInt(1); // Borrowed = -12345678
            }
            return message;
        }
    }
}
