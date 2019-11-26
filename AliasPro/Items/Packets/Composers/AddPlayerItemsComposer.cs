using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Items.Packets.Composers
{
    public class AddPlayerItemsComposer : IPacketComposer
    {
        private readonly IDictionary<int, IList<int>> _itemsToAdd;

        public AddPlayerItemsComposer(int type, int itemId)
        {
			_itemsToAdd = new Dictionary<int, IList<int>>
			{
				{ type, new List<int> { itemId } }
			};
		}

		public AddPlayerItemsComposer(int type, ICollection<int> itemIds)
		{
			_itemsToAdd = new Dictionary<int, IList<int>>
			{
				{ type, new List<int>() }
			};

			foreach (int itemId in itemIds)
			{
				_itemsToAdd[type].Add(itemId);
			}
		}

		public AddPlayerItemsComposer(IDictionary<int, IList<int>> itemsToAdd)
        {
			_itemsToAdd = itemsToAdd;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AddPlayerItemsMessageComposer);
            message.WriteInt(_itemsToAdd.Count);
			foreach (var type in _itemsToAdd)
			{
				message.WriteInt(type.Key);
				message.WriteInt(type.Value.Count);
				foreach (int itemId in type.Value)
				{
					message.WriteInt(itemId);
				}
			}
            return message;
        }
    }
}
