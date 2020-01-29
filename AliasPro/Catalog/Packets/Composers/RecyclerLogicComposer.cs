using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Catalog.Packets.Composers
{
	public class RecyclerLogicComposer : IPacketComposer
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

		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.RecyclerLogicMessageComposer);
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
					message.WriteInt(item.SpriteId);
				}
			}
			return message;
		}
	}
}
