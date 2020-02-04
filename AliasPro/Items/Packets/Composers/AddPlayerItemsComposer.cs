using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Items.Packets.Composers
{
    public class AddPlayerItemsComposer : IMessageComposer
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

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AddPlayerItemsMessageComposer);
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
