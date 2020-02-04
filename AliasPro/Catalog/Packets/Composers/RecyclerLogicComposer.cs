using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Catalog.Packets.Composers
{
	public class RecyclerLogicComposer : IMessageComposer
	{
		private readonly IDictionary<int, int> _recyclerLevels;
		private readonly IDictionary<int, IList<IItemData>> _recyclerPrizes;

		public RecyclerLogicComposer(
			IDictionary<int, int> recyclerLevels,
			IDictionary<int, IList<IItemData>> recyclerPrizes)
		{
			_recyclerLevels = recyclerLevels;
			_recyclerPrizes = recyclerPrizes;
		}

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.RecyclerLogicMessageComposer);
			message.WriteInt(_recyclerPrizes.Count);
			foreach (var prize in _recyclerPrizes)
			{
				message.WriteInt(prize.Key);
				message.WriteInt(_recyclerLevels.ContainsKey(prize.Key) ? _recyclerLevels[prize.Key] : 0);
				message.WriteInt(prize.Value.Count);
				foreach (IItemData item in prize.Value)
				{
					message.WriteString(item.Name);
					message.WriteInt(1); // dunno??
					message.WriteString(item.Type.ToLower());
					message.WriteInt((int)item.SpriteId);
				}
			}
			return message;
		}
	}
}
