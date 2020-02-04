using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Tasks;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Tasks
{
	public class TeleportTaskSix : ITask
	{
		private readonly IItem _item;
		private readonly BaseEntity _entity;

		public TeleportTaskSix(IItem item, BaseEntity entity)
		{
			_item = item;
			_entity = entity;
		}

		public async void Run()
		{
			_item.Mode = 0;
			await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
		}
	}
}
