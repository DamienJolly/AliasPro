using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Utilities;

namespace AliasPro.Items.Interaction
{
    public class InteractionSpinningBottle : ItemInteraction
	{
		private int _tickCount = 0;

		public InteractionSpinningBottle(IItem item)
			: base(item)
		{

        }

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(0);
			message.WriteString(Item.ExtraData);
		}

		public override void OnPlaceItem()
		{
			if (Item.ExtraData == "-1")
				Item.ExtraData = "0";
		}
        
        public async override void OnUserInteract(BaseEntity entity, int state)
        {
			if (entity == null) return;

			if (Item.ExtraData == "-1") return;

			if (!Item.CurrentRoom.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile tile))
				return;

			if (!tile.TilesAdjecent(entity.Position))
			{
				IRoomPosition position = tile.PositionInFront(Item.Rotation);
				entity.GoalPosition = position;
				return;
			}

			Item.ExtraData = "-1";
			_tickCount = 0;
			await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
		}

        public async override void OnCycle()
        {
			if (Item.ExtraData == "-1")
			{
				if (_tickCount >= 6)
				{
					Item.ExtraData = Randomness.RandomNumber(0, Item.ItemData.Modes) + "";
					await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
				}
				_tickCount++;
			}
		}
    }
}
