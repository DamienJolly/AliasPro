namespace AliasPro.Item.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using System.Collections.Generic;

    public class AddPlayerItemsComposer : IPacketComposer
    {
        private readonly ICollection<IItem> _items;

        public AddPlayerItemsComposer(ICollection<IItem> items)
        {
            _items = items;
        }

        public AddPlayerItemsComposer(IItem item)
        {
            _items = new List<IItem>();
            _items.Add(item);
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AddPlayerItemsMessageComposer);
            message.WriteInt(1); //??
            message.WriteInt(1); //??
            message.WriteInt(_items.Count);
            foreach (IItem item in _items)
            {
                message.WriteInt(item.Id);
            }
            return message;
        }
    }
}
