using AliasPro.API.Items.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Entities;
using AliasPro.Tasks;

namespace AliasPro.Items.Tasks
{
	public class TeleportTaskSix : ITask
	{
		private readonly int _roomId;
		private readonly int _playerId;
		private readonly int _itemId;

		public TeleportTaskSix(int roomId, int playerId, int itemId)
		{
			_roomId = roomId;
			_playerId = playerId;
			_itemId = itemId;
		}

		public async void Run()
		{
			if (!Program.GetService<IRoomController>().TryGetRoom((uint)_roomId, out IRoom room))
				return;

			if (!room.Loaded)
			{
				await Program.Tasks.ExecuteTask(new TeleportTaskSix(_roomId, _playerId, _itemId), 1000);
				return;
			}

			if (!room.Items.TryGetItem((uint)_itemId, out IItem item))
				return;

			if (!room.Entities.TryGetPlayerEntityById(_playerId, out PlayerEntity entity))
				return;

			await Program.Tasks.ExecuteTask(new TeleportTaskFour(item, entity), 0);
		}
	}
}
