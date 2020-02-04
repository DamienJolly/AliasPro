using AliasPro.API.Catalog.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Catalog.Packets.Composers
{
	public class GiftConfigurationComposer : IMessageComposer
	{
		private readonly ICollection<ICatalogGiftPart> _gifts;
		private readonly ICollection<ICatalogGiftPart> _wrappers;

		public GiftConfigurationComposer(
			ICollection<ICatalogGiftPart> gifts,
			ICollection<ICatalogGiftPart> wrappers)
		{
			_gifts = gifts;
			_wrappers = wrappers;
		}

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.GiftConfigurationMessageComposer);
			message.WriteBoolean(true); // gifts enabled?
			message.WriteInt(1); // price??
			message.WriteInt(_wrappers.Count);
			foreach (ICatalogGiftPart part in _wrappers)
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

			message.WriteInt(_gifts.Count);
			foreach (ICatalogGiftPart part in _gifts)
				message.WriteInt(part.SpriteId);
			return message;
		}
	}
}
