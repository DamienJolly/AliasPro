using AliasPro.API.Figure.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class UserClothesComposer : IPacketComposer
    {
        private readonly List<int> _itemIds;
        private readonly List<string> _itemNames;

        public UserClothesComposer(ICollection<IClothingItem> items)
        {
			_itemIds = new List<int>();
			_itemNames = new List<string>();

			foreach (IClothingItem item in items)
			{
				foreach (int itemId in item.ClothingIds)
				{
					if (!_itemIds.Contains(itemId))
						_itemIds.Add(itemId);
				}

				if (!_itemNames.Contains(item.Name))
					_itemNames.Add(item.Name);
			}
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserClothesMessageComposer);
            message.WriteInt(_itemIds.Count);
			foreach (int itemId in _itemIds)
			{
				message.WriteInt(itemId);
			}

            message.WriteInt(_itemNames.Count);
			foreach (string itemName in _itemNames)
			{
				message.WriteString(itemName);
			}
			return message;
        }
    }
}
