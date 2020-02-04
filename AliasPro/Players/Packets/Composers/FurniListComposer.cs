using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class FurniListComposer : IMessageComposer
    {
        private readonly ICollection<IItem> _items;

        public FurniListComposer(ICollection<IItem> items)
        {
            _items = items;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.FurniListMessageComposer);
            message.WriteInt(1);
            message.WriteInt(0);

            message.WriteInt(_items.Count);
            foreach (IItem item in _items)
            {
                message.WriteInt((int)item.Id);
                message.WriteString(item.ItemData.Type.ToUpper());
                message.WriteInt((int)item.Id);
                message.WriteInt((int)item.ItemData.SpriteId);
				item.Interaction.Compose(message);
				message.WriteBoolean(item.ItemData.AllowRecycle);
                message.WriteBoolean(item.ItemData.AllowTrade);
                message.WriteBoolean(item.ItemData.AllowInventoryStack);
                message.WriteBoolean(item.ItemData.AllowMarketplace);

                message.WriteInt(-1); //time left
                message.WriteBoolean(false); //renatable period started
                message.WriteInt(-1); //room id

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
