using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Game.Catalog.Packets.Composers
{
	public class RecyclerLogicComposer : IMessageComposer
	{
		private readonly Dictionary<int, int> recyclerLevels;
		private readonly Dictionary<int, List<IItemData>> recyclerPrizes;

		public RecyclerLogicComposer(
			Dictionary<int, int> recyclerLevels,
			Dictionary<int, List<IItemData>> recyclerPrizes)
		{
			this.recyclerLevels = recyclerLevels;
			this.recyclerPrizes = recyclerPrizes;
		}

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.RecyclerLogicMessageComposer);
			message.WriteInt(recyclerPrizes.Count);
			foreach (var prize in recyclerPrizes)
			{
				message.WriteInt(prize.Key);
				message.WriteInt(recyclerLevels.ContainsKey(prize.Key) ? recyclerLevels[prize.Key] : 0);
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
