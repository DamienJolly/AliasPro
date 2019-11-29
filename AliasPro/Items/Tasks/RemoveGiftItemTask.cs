using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Tasks;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Tasks
{
	public class RemoveGiftItemTask : ITask
	{
		private readonly IItemController _itemController;
		private readonly IItem _item;

		public RemoveGiftItemTask(IItemController itemController, IItem item)
		{
			_itemController = itemController;
			_item = item;
		}

		public async void Run()
		{
			if (_item == null) return;

			_item.CurrentRoom.RoomGrid.RemoveItem(_item);
			_item.CurrentRoom.Items.RemoveItem(_item.Id);
			await _item.CurrentRoom.SendAsync(new RemoveFloorItemComposer(_item));
			await _itemController.RemoveItemAsync(_item);
		}
	}
}
