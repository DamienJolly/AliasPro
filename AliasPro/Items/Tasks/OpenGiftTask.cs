using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.API.Tasks;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;
using AliasPro.Tasks;
using System.Collections.Generic;

namespace AliasPro.Items.Tasks
{
	public class OpenGiftTask : ITask
	{
		private readonly IItemController _itemController;
		private readonly IItem _item;
		private readonly ISession _session;

		public OpenGiftTask(IItemController itemController, IItem item, ISession session)
		{
			_itemController = itemController;
			_item = item;
			_session = session;
		}

		public async void Run()
		{
			if (_item == null) return;

			if (_item.Interaction is InteractionGift interaction)
			{
				IItem inside = null;
				IDictionary<int, IList<int>> itemsData = new Dictionary<int, IList<int>>();

				foreach (int giftId in interaction.ItemIds)
				{
					IItem giftItem = await _itemController.GetPlayerItemByIdAsync((uint)giftId);

					if (giftItem == null)
						continue;

					if (!_session.Player.Inventory.TryAddItem(giftItem))
						continue;

					if (inside == null)
						inside = giftItem;

					giftItem.PlayerId = _session.Player.Id;
					giftItem.PlayerUsername = _session.Player.Username;

					if (!itemsData.ContainsKey(1))
						itemsData.Add(1, new List<int>());

					itemsData[1].Add((int)giftItem.Id);
				}

				await _session.SendPacketAsync(new AddPlayerItemsComposer(itemsData));
				await _session.SendPacketAsync(new InventoryRefreshComposer());

				if (inside != null)
				{
					//_session.SendPacketAsync(new InventoryUpdateItemComposer(inside));
					await _session.SendPacketAsync(new PresentItemOpenedComposer(inside, "", false));
				}

				_item.CurrentRoom.RoomGrid.RemoveItem(_item);
				await TaskManager.ExecuteTask(new RemoveGiftItemTask(_itemController, _item), interaction.Exploaded ? 5000 : 0);
			}
		}
	}
}
