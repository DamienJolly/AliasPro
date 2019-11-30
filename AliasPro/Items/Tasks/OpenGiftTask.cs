using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.API.Tasks;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using System;

namespace AliasPro.Items.Tasks
{
	public class OpenGiftTask : ITask
	{
		private readonly IItemData _giftData;
		private readonly string _extraData;
		private readonly IItem _item;
		private readonly ISession _session;

		public OpenGiftTask(IItemData giftData, string extraData, IItem item, ISession session)
		{
			_giftData = giftData;
			_extraData = extraData;
			_item = item;
			_session = session;
		}

		public async void Run()
		{
			if (_item == null) return;

			string extraData = "";
			bool itemIsInRoom = true;

			if (_giftData.InteractionType == ItemInteractionType.TROPHY)
			{
				extraData = _session.Player.Username + "\t" + DateTime.Now.Day + "\t" + DateTime.Now.Month + "\t" + DateTime.Now.Year + "\t" + _extraData;
			}

			_session.CurrentRoom.RoomGrid.RemoveItem(_item);
			_session.CurrentRoom.Items.RemoveItem(_item.Id);
			await _session.CurrentRoom.SendAsync(new RemoveFloorItemComposer(_item));

			_item.ItemData = _giftData;
			_item.ItemId = _giftData.Id;
			_item.ExtraData = extraData;
			_item.Mode = 0;
			_item.Interaction = null;

			if (_item.ItemData.Type.ToLower() == "s")
			{
				_session.CurrentRoom.RoomGrid.AddItem(_item);
				_session.CurrentRoom.Items.AddItem(_item);
				await _session.CurrentRoom.SendAsync(new ObjectAddComposer(_item));
			}
			else
			{
				if (_session.Player.Inventory.TryAddItem(_item))
				{
					_session.CurrentRoom.Items.RemoveItem(_item.Id);

					await _session.SendPacketAsync(new AddPlayerItemsComposer(1, (int)_item.Id));
					await _session.SendPacketAsync(new InventoryRefreshComposer());
				}

				itemIsInRoom = false;
			}

			await _session.SendPacketAsync(new PresentItemOpenedComposer(_item, extraData, itemIsInRoom));
		}
	}
}
