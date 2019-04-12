using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Items.Packets.Composers
{
    public class AddPlayerItemsComposer : IPacketComposer
    {
        private readonly ICollection<IItem> _items;

        public AddPlayerItemsComposer(IItem item)
        {
            _items = new List<IItem> { item };
        }

        public AddPlayerItemsComposer(ICollection<IItem> items)
        {
            _items = items;
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
