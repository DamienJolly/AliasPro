using AliasPro.API.Figure.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using System.Collections.Generic;

namespace AliasPro.Figure.Packets.Composers
{
    public class UserWardrobeComposer : IPacketComposer
    {
		private readonly ICollection<IWardrobeItem> _items;

		public UserWardrobeComposer(ICollection<IWardrobeItem> items)
		{
			_items = items;
		}

		public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserWardrobeMessageComposer);

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
