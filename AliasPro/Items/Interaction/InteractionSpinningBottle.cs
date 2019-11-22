using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;
using AliasPro.Utilities;

namespace AliasPro.Items.Interaction
{
    public class InteractionSpinningBottle : IItemInteractor
    {
        private readonly IItem _item;

		private int _tickCount = 0;

		public InteractionSpinningBottle(IItem item)
        {
            _item = item;
        }

        public void Compose(ServerPacket message)
        {
			message.WriteInt(0);
			message.WriteString(_item.Mode.ToString());
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

			if (_item.Mode == -1) return;

			if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
				return;

			if (!tile.TilesAdjecent(entity.Position))
			{
				IRoomPosition position = tile.PositionInFront(_item.Rotation);
				entity.GoalPosition = position;
				return;
			}

			_item.Mode = -1;
			_tickCount = 0;
			await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
		}

        public async void OnCycle()
        {
			if (_item.Mode == -1)
			{
				if (_tickCount >= 6)
				{
					_item.Mode = Randomness.RandomNumber(0, _item.ItemData.Modes);
					await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
				}
				_tickCount++;
			}
		}
    }
}
