using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Tasks;
using AliasPro.Rooms.Entities;
using AliasPro.Tasks;

namespace AliasPro.Items.Interaction
{
    public class InteractionTeleport : ItemInteraction
	{
		public int Mode;

		public InteractionTeleport(IItem item)
			: base(item)
        {
			Mode = 0;
        }

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(0);
			message.WriteString(Mode.ToString());
		}

		public override void OnPlaceItem()
		{
			Mode = 0;
		}

		public override void OnMoveItem()
		{
			Mode = 0;
		}
        
        public async override void OnUserInteract(BaseEntity entity, int state)
        {
			if (entity == null) return;

			if (!(entity is PlayerEntity playerEntity)) return;

			if (Item.ItemData.CanWalk) return;

			if (!Item.CurrentRoom.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile tile))
				return;

			IRoomPosition position = tile.PositionInFront(Item.Rotation);
			if (!(position.X == playerEntity.Position.X && position.Y == playerEntity.Position.Y))
			{
				playerEntity.GoalPosition = position;
				return;
			}

			Item.ItemData.CanWalk = true;
			Mode = 1;
			playerEntity.GoalPosition = Item.Position;
			await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
			await TaskManager.ExecuteTask(new TeleportTaskOne(Item, playerEntity), 1500);
		}
    }
}
