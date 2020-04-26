using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionOneWayGate : ItemInteraction
	{
		public InteractionOneWayGate(IItem item)
			: base(item)
		{

        }

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(0);
			message.WriteString(Item.ExtraData);
		}

		public override void OnPickupItem()
		{
			Item.ExtraData = "0";
		}
        
        public async override void OnUserInteract(BaseEntity entity, int state)
        {
			if(entity == null) return;

			if (Item.ExtraData == "1") return;

			if (!Item.CurrentRoom.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile tile))
				return;

			IRoomPosition position = tile.PositionInFront(Item.Rotation);
			if (!(position.X == entity.Position.X && position.Y == entity.Position.Y))
			{
				entity.GoalPosition = position;
				return;
			}

			if (!entity.Actions.HasStatus("sit") &&
				!entity.Actions.HasStatus("lay"))
			{
				entity.Actions.RemoveStatus("mv");
				entity.SetRotation(entity.Position.CalculateDirection(Item.Position.X, Item.Position.Y));
			}

			int newRot = Item.Rotation + 4;
			if (newRot > 6)
				newRot -= 8;

			IRoomPosition newPosition = tile.PositionInFront(newRot);
			entity.GoalPosition = newPosition;

			Item.ExtraData = "1";
			Item.InteractingPlayer = entity;

			await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
        }

		public async override void OnCycle()
		{
			if (Item.ExtraData != "1") return;

			if (!Item.CurrentRoom.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile tile))
				return;

			int newRot = Item.Rotation + 4;
			if (newRot > 6)
				newRot -= 8;

			IRoomPosition newPosition = tile.PositionInFront(newRot);

			if (newPosition.X != Item.InteractingPlayer.GoalPosition.X || newPosition.Y != Item.InteractingPlayer.GoalPosition.Y ||
				newPosition.X == Item.InteractingPlayer.Position.X && newPosition.Y == Item.InteractingPlayer.Position.Y)
			{
				Item.ExtraData = "0";
				Item.InteractingPlayer = null;
				await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
				return;
			}
		}
	}
}
