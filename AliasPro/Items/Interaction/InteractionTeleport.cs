using AliasPro.API.Items.Interaction;
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
    public class InteractionTeleport : IItemInteractor
    {
        private readonly IItem _item;
		public int Mode;

		public InteractionTeleport(IItem item)
        {
            _item = item;
			Mode = 0;
        }

		public void Compose(ServerMessage message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(1);
			message.WriteInt(0);
            message.WriteString(Mode.ToString());
        }

		public void OnPlaceItem()
		{

		}

		public void OnPickupItem()
		{
			Mode = 0;
		}

		public void OnMoveItem()
		{
			Mode = 0;
		}

		public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

		}
        
        public async void OnUserInteract(BaseEntity entity, int state)
        {
			if (entity == null) return;

			if (!(entity is PlayerEntity playerEntity)) return;

			if (_item.ItemData.CanWalk) return;

			if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
				return;

			IRoomPosition position = tile.PositionInFront(_item.Rotation);
			if (!(position.X == playerEntity.Position.X && position.Y == playerEntity.Position.Y))
			{
				playerEntity.GoalPosition = position;
				return;
			}

			_item.ItemData.CanWalk = true;
			Mode = 1;
			playerEntity.GoalPosition = _item.Position;
			await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
			await TaskManager.ExecuteTask(new TeleportTaskOne(_item, playerEntity), 1500);
		}

        public void OnCycle()
        {

		}
    }
}
