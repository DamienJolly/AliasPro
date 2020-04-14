using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Entities;
using AliasPro.Tasks;

namespace AliasPro.Items.Tasks
{
	public class TeleportTaskFive : ITask
	{
		private readonly IItem _item;
		private readonly PlayerEntity _entity;

		public TeleportTaskFive(IItem item, PlayerEntity entity)
		{
			_item = item;
			_entity = entity;
		}

		public async void Run()
		{
			if (_item.Interaction is InteractionTeleport teleportInteraction)
			{
				if (_item.Position.X != _entity.Position.X || _item.Position.Y != _entity.Position.Y ||
				!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
				{
					teleportInteraction.Mode = 0;
					await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
					return;
				}

				_entity.SetRotation(_item.Rotation);
				_entity.GoalPosition = tile.PositionInFront(_item.Rotation);
				teleportInteraction.Mode = 1;

				await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
				await TaskManager.ExecuteTask(new TeleportTaskFive(_item, _entity), 1500);
			}
		}
	}
}
