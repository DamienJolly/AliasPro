using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Tasks;
using AliasPro.Items.Packets.Composers;
using AliasPro.Tasks;

namespace AliasPro.Items.Tasks
{
	public class TeleportTaskFour : ITask
	{
		private readonly IItem _item;
		private readonly BaseEntity _entity;

		public TeleportTaskFour(IItem item, BaseEntity entity)
		{
			_item = item;
			_entity = entity;
		}

		public async void Run()
		{
			_item.CurrentRoom.RoomGrid.RemoveEntity(_entity);
			_entity.Position = _entity.NextPosition = _entity.GoalPosition = _item.Position;
			_entity.SetRotation(_item.Rotation);
			_item.CurrentRoom.RoomGrid.AddEntity(_entity);
			_item.Mode = 2;

			await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
			await TaskManager.ExecuteTask(new TeleportTaskFive(_item, _entity), 2000);
		}
	}
}
