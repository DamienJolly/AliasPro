using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using System;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Purchase.Handlers
{
	public class BadgeDisplayPurchaseHandler : IPurchaseHandler
	{
		public async Task<bool> TryHandlePurchase(ISession session, string extraData, IItemData itemData, int amount)
		{
			if (!session.Player.Badge.HasBadge(extraData))
				return false;

			extraData = extraData + ";" + session.Player.Username + ";" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;

			IItem playerItem = new Item(itemData.Id, session.Player.Id, session.Player.Username, extraData, itemData);
			playerItem.Id = (uint)await Program.GetService<IItemController>().AddNewItemAsync(playerItem);
			session.Player.Inventory.TryAddItem(playerItem);

			await session.SendPacketAsync(new AddPlayerItemsComposer(1, (int)playerItem.Id));
			return true;
		}
	}
}
