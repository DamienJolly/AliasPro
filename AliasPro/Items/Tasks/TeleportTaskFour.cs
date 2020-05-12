using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Entities;
using AliasPro.Tasks;

namespace AliasPro.Items.Tasks
{
	public class TeleportTaskFour : ITask
	{
		private readonly IItem _item;
		private readonly PlayerEntity _entity;

		public TeleportTaskFour(IItem item, PlayerEntity entity)
		{
			_item = item;
			_entity = entity;
		}

		public async void Run()
		{
			if (_item.Interaction is InteractionTeleport teleportInteraction)
			{
				_item.CurrentRoom.RoomGrid.RemoveEntity(_entity);
				_entity.Position = _entity.NextPosition = _entity.GoalPosition = _item.Position;
				_entity.SetRotation(_item.Rotation);
				_item.CurrentRoom.RoomGrid.AddEntity(_entity);
				teleportInteraction.Mode = 2;

				await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
				await Program.Tasks.ExecuteTask(new TeleportTaskFive(_item, _entity), 2000);
			}
		}
	}
}
