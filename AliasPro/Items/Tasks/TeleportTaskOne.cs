﻿using AliasPro.API.Items.Models;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Entities;
using AliasPro.Tasks;

namespace AliasPro.Items.Tasks
{
	public class TeleportTaskOne : ITask
	{
		private readonly IItem _item;
		private readonly PlayerEntity _entity;

		public TeleportTaskOne(IItem item, PlayerEntity entity)
		{
			_item = item;
			_entity = entity;
		}

		public async void Run()
		{
			if (_item.Interaction is InteractionTeleport teleportInteraction)
			{
				if (_item.Position.X != _entity.Position.X || _item.Position.Y != _entity.Position.Y)
				{
					_item.ItemData.CanWalk = false;
					teleportInteraction.Mode = 0;
					await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
					return;
				}

				_item.ItemData.CanWalk = false;
				teleportInteraction.Mode = 0;

				await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
				await Program.Tasks.ExecuteTask(new TeleportTaskTwo(_item, _entity), 1500);
			}
		}
	}
}
