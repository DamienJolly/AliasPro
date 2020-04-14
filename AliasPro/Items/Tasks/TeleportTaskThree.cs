using AliasPro.API.Items.Models;
using AliasPro.API.Tasks;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Tasks
{
	public class TeleportTaskThree : ITask
	{
		private readonly IItem _item;

		public TeleportTaskThree(IItem item)
		{
			_item = item;
		}

		public async void Run()
		{
			if (_item.Interaction is InteractionTeleport teleportInteraction)
			{
				teleportInteraction.Mode = 0;
				await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
			}
		}
	}
}
