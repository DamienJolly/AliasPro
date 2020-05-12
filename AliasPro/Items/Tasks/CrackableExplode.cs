using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Tasks;

namespace AliasPro.Items.Tasks
{
	public class CrackableExplode : ITask
	{
		private readonly IItem _item;
		private readonly ISession _session;
		private bool _placeInRoom;

		public CrackableExplode(IItem item, ISession session, bool placeInRoom)
		{
			_item = item;
			_session = session;
			_placeInRoom = placeInRoom;
		}

		public async void Run()
		{
			if (_item == null) return;

			_session.CurrentRoom.RoomGrid.RemoveItem(_item);
			_session.CurrentRoom.Items.RemoveItem(_item.Id);
			await _session.CurrentRoom.SendPacketAsync(new RemoveFloorItemComposer(_item, true));
			await Program.GetService<IItemController>().RemoveItemAsync(_item);

			if (!Program.GetService<IItemController>().TryGetCrackableDataById((int)_item.ItemData.Id, out ICrackableData crackable))
				return;

			if (!crackable.TryGetCrackableReward(out int prizeId))
				return;

			if (!Program.GetService<IItemController>().TryGetItemDataById((uint)prizeId, out IItemData prizeData))
				return;

			IItem prizeItem = new Item((uint)prizeData.Id, _session.Player.Id, _session.Player.Username, "", prizeData);
			prizeItem.Id = (uint)await Program.GetService<IItemController>().AddNewItemAsync(prizeItem);

			if (_placeInRoom && prizeData.Type == "s")
			{
				prizeItem.Position.X = _item.Position.X;
				prizeItem.Position.Y = _item.Position.Y;
				prizeItem.Position.Z = _item.Position.Z;
				prizeItem.RoomId = prizeItem.RoomId;
				_session.CurrentRoom.RoomGrid.AddItem(prizeItem);
				_session.CurrentRoom.Items.AddItem(prizeItem);
				await _session.CurrentRoom.SendPacketAsync(new ObjectAddComposer(prizeItem));
			}
			else
			{
				if (_session.Player.Inventory.TryAddItem(_item))
				{
					_session.CurrentRoom.Items.RemoveItem(_item.Id);

					await _session.SendPacketAsync(new AddPlayerItemsComposer(1, (int)_item.Id));
					await _session.SendPacketAsync(new InventoryRefreshComposer());
				}

				_placeInRoom = false;
			}
		}
	}
}
