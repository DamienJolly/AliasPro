using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Interaction
{
    public class InteractionOneWayGate : IItemInteractor
    {
        private readonly IItem _item;

		public InteractionOneWayGate(IItem item)
        {
            _item = item;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteInt(0);
            message.WriteString(_item.Mode.ToString());
        }

		public void OnPlaceItem()
		{

		}

		public void OnPickupItem()
		{
			_item.Mode = 0;
		}

		public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

        }
        
        public async void OnUserInteract(BaseEntity entity, int state)
        {
			if(entity == null) return;

			if (_item.Mode == 1) return;

			if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
				return;

			IRoomPosition position = tile.PositionInFront(_item.Rotation);
			if (!(position.X == entity.Position.X && position.Y == entity.Position.Y))
			{
				entity.GoalPosition = position;
				return;
			}

			if (!entity.Actions.HasStatus("sit") &&
				!entity.Actions.HasStatus("lay"))
			{
				entity.Actions.RemoveStatus("mv");
				entity.SetRotation(entity.Position.CalculateDirection(_item.Position.X, _item.Position.Y));
			}

			int newRot = _item.Rotation + 4;
			if (newRot > 6)
				newRot -= 8;

			IRoomPosition newPosition = tile.PositionInFront(newRot);
			entity.GoalPosition = newPosition;

			_item.Mode = 1;
			_item.InteractingPlayer = entity;

			await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
        }

		public async void OnCycle()
		{
			if (_item.Mode != 1) return;

			if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
				return;

			int newRot = _item.Rotation + 4;
			if (newRot > 6)
				newRot -= 8;

			IRoomPosition newPosition = tile.PositionInFront(newRot);

			if (newPosition.X != _item.InteractingPlayer.GoalPosition.X || newPosition.Y != _item.InteractingPlayer.GoalPosition.Y ||
				newPosition.X == _item.InteractingPlayer.Position.X && newPosition.Y == _item.InteractingPlayer.Position.Y)
			{
				_item.Mode = 0;
				_item.InteractingPlayer = null;
				await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
				return;
			}
		}
	}
}
