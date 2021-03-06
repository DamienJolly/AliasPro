﻿using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class FloorPlanSaveEvent : IMessageEvent
    {
        public short Header => Incoming.FloorPlanSaveMessageEvent;

		private readonly IRoomController _roomController;

		public FloorPlanSaveEvent(IRoomController roomController)
		{
			_roomController = roomController;
		}

		public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

			if (room.OwnerId != session.Player.Id) return;

			IList<string> errors = new List<string>();

			string map = message.ReadString().ToLower().TrimEnd();
			if (map.Length == 0)
			{
				errors.Add("${notification.floorplan_editor.error.message.effective_height_is_0}");
			}

			int lengthX = -1;
			int lengthY = -1;

			string[] data = map.Split('\r');
			if (errors.Count == 0)
			{
				if (map.Length > 64 * 64)
				{
					errors.Add("${notification.floorplan_editor.error.message.too_large_area}");
				}


				lengthY = data.Length;
				if (data.Length > 64)
				{
					errors.Add("${notification.floorplan_editor.error.message.too_large_height}");
				}
				else
				{
					foreach (string s in data)
					{
						if (lengthX == -1)
							lengthX = s.Length;

						if (s.Length != lengthX)
							break;

						if (s.Length > 64 || s.Length == 0)
						{
							errors.Add("${notification.floorplan_editor.error.message.too_large_width}");
						}
					}
				}
			}

			int doorX = message.ReadInt();
			int doorY = message.ReadInt();

			if (doorX < 0 || doorX > lengthX || doorY < 0 || doorY > lengthY || data[doorY][doorX] == 'x')
			{
				errors.Add("${notification.floorplan_editor.error.message.entry_tile_outside_map}");
			}

			int doorRotation = message.ReadInt();

			if (doorRotation < 0 || doorRotation > 7)
			{
				errors.Add("${notification.floorplan_editor.error.message.invalid_entry_tile_direction}");
			}

			int wallSize = message.ReadInt();
			if (wallSize < -2 || wallSize > 1)
			{
				errors.Add("${notification.floorplan_editor.error.message.invalid_wall_thickness}");
			}

			int floorSize = message.ReadInt();
			if (floorSize < -2 || floorSize > 1)
			{
				errors.Add("${notification.floorplan_editor.error.message.invalid_floor_thickness}");
			}

			int wallHeight = -1;
			if (message.BytesAvailable() >= 4)
				wallHeight = message.ReadInt();

			if (wallHeight < -1 || wallHeight > 15)
			{
				errors.Add("${notification.floorplan_editor.error.message.invalid_walls_fixed_height}");
			}

			if (errors.Count != 0)
			{
				StringBuilder errorMessage = new StringBuilder();
				foreach (string error in errors)
				{
					errorMessage.Append(error).Append("<br />");
				}

				await session.SendPacketAsync(new BubbleAlertComposer(BubbleAlertComposer.FLOORPLAN_EDITOR_ERROR, errorMessage.ToString()));
				return;
			}

			IRoomModel model = room.RoomModel;
			if (model.IsCustom)
			{
				model.DoorX = doorX;
				model.DoorY = doorY;
				model.DoorDir = doorRotation;
				model.HeightMap = map;
				model.InitializeHeightMap();
				await _roomController.UpdateRoomModel(model);
			}
			else
			{
				IRoomModel newModel = new RoomModel("model_bc_" + room.Id, map, doorX, doorY, doorRotation);
				if (await _roomController.TryAddRoomModel(newModel))
				{
					room.RoomModel = newModel;
					room.ModelName = newModel.Id;
				}
			}

			room.RoomGrid = new RoomGrid(room);
			room.Items.InitializeItems();
			room.WallHeight = wallHeight;
			room.Settings.FloorThickness = floorSize;
			room.Settings.WallThickness = wallSize;

			await _roomController.UpdateRoom(room);

			foreach (BaseEntity entity in room.Entities.Entities)
			{
				if (!(entity is PlayerEntity playerEntity)) continue;

				await playerEntity.Session.SendPacketAsync(new ForwardToRoomComposer(room.Id));
			}
        }
    }
}
