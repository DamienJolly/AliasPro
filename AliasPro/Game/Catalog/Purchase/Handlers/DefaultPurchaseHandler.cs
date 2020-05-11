using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Purchase.Handlers
{
	public class DefaultPurchaseHandler : IPurchaseHandler
	{
		public async Task<bool> TryHandlePurchase(ISession session, string extraData, IItemData itemData, int amount)
		{
			List<int> itemIds = new List<int>();

			for (int k = 0; k < amount; k++)
			{
				IItem playerItem = new Item(itemData.Id, session.Player.Id, session.Player.Username, "", itemData);
				playerItem.Id = (uint)await Program.GetService<IItemController>().AddNewItemAsync(playerItem);

				if (session.Player.Inventory.TryAddItem(playerItem))
					itemIds.Add((int)playerItem.Id);
			}

			await session.SendPacketAsync(new AddPlayerItemsComposer(1, itemIds));
			return true;
		}
	}
}
