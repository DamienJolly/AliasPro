using AliasPro.API.Figure.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Types;
using System.Collections.Generic;

namespace AliasPro.Figure.Packets.Composers
{
    public class UserWardrobeComposer : IMessageComposer
    {
		private readonly ICollection<IWardrobeItem> _items;

		public UserWardrobeComposer(ICollection<IWardrobeItem> items)
		{
			_items = items;
		}

		public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserWardrobeMessageComposer);

			message.WriteInt(1); // can use wardrobe?
			message.WriteInt(_items.Count);
			foreach (IWardrobeItem item in _items)
			{
				message.WriteInt(item.SlotId);
				message.WriteString(item.Figure);
				message.WriteString(item.Gender == PlayerGender.MALE ? "m" : "f");
			}
            return message;
        }
    }
}
