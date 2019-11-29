using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionLoveLock : IItemInteractor
    {
        private readonly IItem _item;

		public InteractionLoveLock(IItem item)
        {
            _item = item;
        }

		public void Compose(ServerPacket message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(2);
			message.WriteInt(6);

			if (!string.IsNullOrEmpty(_item.ExtraData))
			{
				string[] data = _item.ExtraData.Split(";");

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

		public void OnPlaceItem()
		{

		}

		public void OnPickupItem()
		{

		}

		public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

        }
        
        public async void OnUserInteract(BaseEntity entity, int state)
        {
			if (!string.IsNullOrEmpty(_item.ExtraData))
				return;

			if (!(entity is PlayerEntity playerEntity))
				return;

			if (!CanLoveLock(entity))
				return;

			if (_item.PlayerId == playerEntity.Player.Id)
			{
				_item.InteractingPlayer = playerEntity;
				await playerEntity.Player.Session.SendPacketAsync(new LoveLockStartComposer(_item));
			}
			else
			{
				_item.InteractingPlayerTwo = playerEntity;
				await playerEntity.Player.Session.SendPacketAsync(new LoveLockStartComposer(_item));
			}
        }

        public void OnCycle()
        {

        }

		private bool CanLoveLock(BaseEntity entity)
		{
			int leftRot = _item.Rotation - 2;
			int rightRot = _item.Rotation + 2;

			if (leftRot < 0) leftRot = 6;
			if (rightRot > 6) rightRot = 0;

			if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
				return false;

			if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(tile.PositionInFront(leftRot).X, tile.PositionInFront(leftRot).Y, out IRoomTile leftTile))
				return false;

			if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(tile.PositionInFront(rightRot).X, tile.PositionInFront(rightRot).Y, out IRoomTile rightTile))
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
