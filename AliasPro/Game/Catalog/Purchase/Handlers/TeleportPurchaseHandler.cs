using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Purchase.Handlers
{
	public class TeleportPurchaseHandler : IPurchaseHandler
	{
		public async Task<bool> TryHandlePurchase(ISession session, string extraData, IItemData itemData, int amount)
		{
			List<int> itemsIds = new List<int>();

			IItem teleportOne = new Item((uint)itemData.Id, session.Player.Id, session.Player.Username, "", itemData);
			teleportOne.Id = (uint)await Program.GetService<IItemController>().AddNewItemAsync(teleportOne);

			if (session.Player.Inventory.TryAddItem(teleportOne))
				itemsIds.Add((int)teleportOne.Id);

			IItem teleportTwo = new Item((uint)itemData.Id, session.Player.Id, session.Player.Username, "", itemData);
			teleportTwo.Id = (uint)await Program.GetService<IItemController>().AddNewItemAsync(teleportTwo);

			if (session.Player.Inventory.TryAddItem(teleportTwo))
				itemsIds.Add((int)teleportTwo.Id);

			//todo: better tele links

			await session.SendPacketAsync(new AddPlayerItemsComposer(1, itemsIds));
			return true;
		}
	}
}
