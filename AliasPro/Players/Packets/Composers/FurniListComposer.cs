using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class FurniListComposer : IPacketComposer
    {
        private readonly ICollection<IItem> _items;

        public FurniListComposer(ICollection<IItem> items)
        {
            _items = items;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.FurniListMessageComposer);
            message.WriteInt(1);
            message.WriteInt(0);

            message.WriteInt(_items.Count);
            foreach (IItem item in _items)
            {
                message.WriteInt(item.Id);
                message.WriteString(item.ItemData.Type.ToUpper());
                message.WriteInt(item.Id);
                message.WriteInt(item.ItemData.SpriteId);
				item.Interaction.Compose(message);
				message.WriteBoolean(false); //Allow ecotron.
                message.WriteBoolean(true); //Allow trade
                message.WriteBoolean(true); //Allow inventory stack
                message.WriteBoolean(true); //Allow the item to be sold on the market place.

                message.WriteInt(-1);
                message.WriteBoolean(false);
                message.WriteInt(-1);

                if (item.ItemData.Type != "i")
                {
                    message.WriteString("");
                    message.WriteInt(0);
                }
            }
            return message;
        }
    }
}
