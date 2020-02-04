using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Items.Packets.Composers;
using AliasPro.Tasks;

namespace AliasPro.Items.Tasks
{
	public class TeleportTaskTwo : ITask
	{
		private readonly IItem _item;
		private readonly BaseEntity _entity;

		public TeleportTaskTwo(IItem item, BaseEntity entity)
		{
			_item = item;
			_entity = entity;
		}

		public async void Run()
		{
			if (_item.Position.X != _entity.Position.X || _item.Position.Y != _entity.Position.Y) return;

			if (_item.CurrentRoom.Items.TryGetItem(uint.Parse(_item.ExtraData), out IItem otherItem))
			{
				_item.Mode = 2;
				await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
				await TaskManager.ExecuteTask(new TeleportTaskFour(otherItem, _entity), 1000);
				await TaskManager.ExecuteTask(new TeleportTaskThree(_item, _entity), 1000);
			}
			else
			{
				if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
					return;

				_entity.GoalPosition = tile.PositionInFront(_item.Rotation);

				_item.Mode = 1;
				await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
				await TaskManager.ExecuteTask(new TeleportTaskOne(_item, _entity), 1000);
			}
		}
	}
}
