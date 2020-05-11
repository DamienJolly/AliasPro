using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;
using System.Collections.Generic;

namespace AliasPro.Game.Catalog.Packets.Composers
{
	public class GiftConfigurationComposer : IMessageComposer
	{
		private readonly ICollection<CatalogGiftPartData> gifts;
		private readonly ICollection<CatalogGiftPartData> wrappers;

		public GiftConfigurationComposer(
			ICollection<CatalogGiftPartData> gifts,
			ICollection<CatalogGiftPartData> wrappers)
		{
			this.gifts = gifts;
			this.wrappers = wrappers;
		}

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.GiftConfigurationMessageComposer);
			message.WriteBoolean(true); // gifts enabled?
			message.WriteInt(1); // price??

			message.WriteInt(wrappers.Count);
			foreach (CatalogGiftPartData part in wrappers)
				message.WriteInt(part.SpriteId);

			message.WriteInt(8);
			{
				message.WriteInt(0);
				message.WriteInt(1);
				message.WriteInt(2);
				message.WriteInt(3);
				message.WriteInt(4);
				message.WriteInt(5);
				message.WriteInt(6);
				message.WriteInt(8);
			}

			message.WriteInt(11);
			{
				message.WriteInt(0);
				message.WriteInt(1);
				message.WriteInt(2);
				message.WriteInt(3);
				message.WriteInt(4);
				message.WriteInt(5);
				message.WriteInt(6);
				message.WriteInt(7);
				message.WriteInt(8);
				message.WriteInt(9);
				message.WriteInt(10);
			}

			message.WriteInt(gifts.Count);
			foreach (CatalogGiftPartData part in gifts)
				message.WriteInt(part.SpriteId);

			return message;
		}
	}
}
