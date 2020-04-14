using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Tasks;

namespace AliasPro.Items.Tasks
{
	public class TeleportTaskTwo : ITask
	{
		private readonly IItem _item;
		private readonly PlayerEntity _entity;

		public TeleportTaskTwo(IItem item, PlayerEntity entity)
		{
			_item = item;
			_entity = entity;
		}

		public async void Run()
		{
			if (_item.Interaction is InteractionTeleport teleportInteraction)
			{
				if (_item.Position.X != _entity.Position.X || _item.Position.Y != _entity.Position.Y) return;

				if (!uint.TryParse(_item.ExtraData, out uint linkId)) return;

				if (_item.CurrentRoom.Items.TryGetItem(linkId, out IItem otherItem))
				{
					if (otherItem.ItemData.InteractionType == ItemInteractionType.TELEPORT)
					{
						teleportInteraction.Mode = 2;
						await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
						await TaskManager.ExecuteTask(new TeleportTaskFour(otherItem, _entity), 1000);
						await TaskManager.ExecuteTask(new TeleportTaskThree(_item), 1000);
					}
				}
				else
				{
					otherItem =
						await Program.GetService<IItemController>().GetPlayerItemByIdAsync(linkId);

					if (otherItem != null && otherItem.RoomId != 0 && otherItem.ItemData.InteractionType == ItemInteractionType.TELEPORT)
					{
						await _entity.Session.SendPacketAsync(new ForwardToRoomComposer(otherItem.RoomId));
						teleportInteraction.Mode = 2;
						await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
						await TaskManager.ExecuteTask(new TeleportTaskSix((int)otherItem.RoomId, (int)_entity.Player.Id, (int)otherItem.Id), 1000);
						await TaskManager.ExecuteTask(new TeleportTaskThree(_item), 1000);
					}
					else
					{
						if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
							return;

						_entity.GoalPosition = tile.PositionInFront(_item.Rotation);

						teleportInteraction.Mode = 1;
						await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
						await TaskManager.ExecuteTask(new TeleportTaskOne(_item, _entity), 1000);
					}
				}





				/*	if (_item.CurrentRoom.Items.TryGetItem(uint.Parse(_item.ExtraData), out IItem otherItem))
				{
					teleportInteraction.Mode = 2;
					await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
					await TaskManager.ExecuteTask(new TeleportTaskFour(otherItem, _entity), 1000);
					await TaskManager.ExecuteTask(new TeleportTaskThree(_item, _entity), 1000);
				}
				else if (true)
				{
					IItem otherItemTwo = 
						await Program.GetService<IItemController>().GetPlayerItemByIdAsync(uint.Parse(_item.ExtraData));
					teleportInteraction.Mode = 2;
					await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
					await TaskManager.ExecuteTask(new TeleportTaskFour(otherItem, _entity), 1000);
					await TaskManager.ExecuteTask(new TeleportTaskThree(_item, _entity), 1000);
				}
				else
				{
					if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
						return;

					_entity.GoalPosition = tile.PositionInFront(_item.Rotation);

					teleportInteraction.Mode = 1;
					await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
					await TaskManager.ExecuteTask(new TeleportTaskOne(_item, _entity), 1000);
				}*/
			}
		}
	}
}
