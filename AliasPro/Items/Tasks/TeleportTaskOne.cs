using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Tasks;
using AliasPro.Items.Packets.Composers;
using AliasPro.Tasks;

namespace AliasPro.Items.Tasks
{
	public class TeleportTaskOne : ITask
	{
		private readonly IItem _item;
		private readonly BaseEntity _entity;

		public TeleportTaskOne(IItem item, BaseEntity entity)
		{
			_item = item;
			_entity = entity;
		}

		public async void Run()
		{
			if (_item.Position.X != _entity.Position.X || _item.Position.Y != _entity.Position.Y)
			{
				_item.ItemData.CanWalk = false;
				_item.Mode = 0;
				await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
				return;
			}

			_item.ItemData.CanWalk = false;
			_item.Mode = 0;

			await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
			await TaskManager.ExecuteTask(new TeleportTaskTwo(_item, _entity), 1500);
		}
	}
}
