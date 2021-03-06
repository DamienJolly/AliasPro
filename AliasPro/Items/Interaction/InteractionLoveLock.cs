﻿using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionLoveLock : ItemInteraction
	{
		public InteractionLoveLock(IItem item)
			: base(item)
        {

        }

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(2);
			message.WriteInt(6);

			if (!string.IsNullOrEmpty(Item.ExtraData))
			{
				string[] data = Item.ExtraData.Split(";");

				message.WriteString("1");
				message.WriteString(data[0]);
				message.WriteString(data[1]);
				message.WriteString(data[2]);
				message.WriteString(data[3]);
				message.WriteString(data[4]);
			}
			else
			{
				message.WriteString("0");
				message.WriteString(string.Empty);
				message.WriteString(string.Empty);
				message.WriteString(string.Empty);
				message.WriteString(string.Empty);
				message.WriteString(string.Empty);
			}
		}
        
        public async override void OnUserInteract(BaseEntity entity, int state)
        {
			if (!string.IsNullOrEmpty(Item.ExtraData))
				return;

			if (!(entity is PlayerEntity playerEntity))
				return;

			if (!CanLoveLock(entity))
				return;

			if (Item.PlayerId == playerEntity.Player.Id)
			{
				Item.InteractingPlayer = playerEntity;
				await playerEntity.Player.Session.SendPacketAsync(new LoveLockStartComposer(Item));
			}
			else
			{
				Item.InteractingPlayerTwo = playerEntity;
				await playerEntity.Player.Session.SendPacketAsync(new LoveLockStartComposer(Item));
			}
        }

		private bool CanLoveLock(BaseEntity entity)
		{
			int leftRot = Item.Rotation - 2;
			int rightRot = Item.Rotation + 2;

			if (leftRot < 0) leftRot = 6;
			if (rightRot > 6) rightRot = 0;

			if (!Item.CurrentRoom.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile tile))
				return false;

			if (!Item.CurrentRoom.RoomGrid.TryGetRoomTile(tile.PositionInFront(leftRot).X, tile.PositionInFront(leftRot).Y, out IRoomTile leftTile))
				return false;

			if (!Item.CurrentRoom.RoomGrid.TryGetRoomTile(tile.PositionInFront(rightRot).X, tile.PositionInFront(rightRot).Y, out IRoomTile rightTile))
				return false;

			if ((leftTile.Position.X == entity.Position.X && leftTile.Position.Y == entity.Position.Y) ||
				(rightTile.Position.X == entity.Position.X && rightTile.Position.Y == entity.Position.Y))
				return true;
			
			if (!(leftTile.Position.X == entity.Position.X && leftTile.Position.Y == entity.Position.Y) && leftTile.Entities.Count == 0)
				entity.GoalPosition = leftTile.Position;
			else if (!(rightTile.Position.X == entity.Position.X && rightTile.Position.Y == entity.Position.Y) && rightTile.Entities.Count == 0)
				entity.GoalPosition = rightTile.Position;

			return false;
		}
    }
}
