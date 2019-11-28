using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionTeleport : IItemInteractor
    {
        private readonly IItem _item;

		private int _cycle = 0;
		private int _step = 0;

		public InteractionTeleport(IItem item)
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
			_step = 0;
			_item.InteractingPlayer = null;
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

			if (_item.InteractingPlayer != null) return;

			if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
				return;

			IRoomPosition position = tile.PositionInFront(_item.Rotation);
			if (!(position.X == entity.Position.X && position.Y == entity.Position.Y))
			{
				entity.GoalPosition = position;
				return;
			}

			entity.GoalPosition = _item.Position;

			_item.Mode = 1;
			_cycle = 0;
			_step = 1;
			_item.InteractingPlayer = entity;

			await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
		}

        public async void OnCycle()
        {
			if (_item.InteractingPlayer == null) return;

			switch (_step)
			{
				case 0:
					{
						_item.Mode = 2;
						await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
						_step = 6;
						_cycle = 2;
						break;
					}
				case 1:
					{
						if (_item.Position.X == _item.InteractingPlayer.Position.X && _item.Position.Y == _item.InteractingPlayer.Position.Y)
						{
							_step = 2;
							_cycle = 1;
						}
						break;
					}
				case 2:
					{
						if (_cycle <= 0)
						{
							_item.Mode = 0;
							await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
							_step = 3;
							_cycle = 2;
						}
						break;
					}
				case 3:
					{
						if (_cycle <= 0)
						{
							_item.Mode = 2;
							await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
							_step = 4;
							_cycle = 1;
						}
						break;
					}
				case 4:
					{
						if (_cycle <= 0)
						{
							if (_item.CurrentRoom.Items.TryGetItem(uint.Parse(_item.ExtraData), out IItem otherItem))
							{
								otherItem.InteractingPlayer = _item.InteractingPlayer;
								_item.InteractingPlayer.Position = 
									_item.InteractingPlayer.NextPosition =
									_item.InteractingPlayer.GoalPosition = otherItem.Position;
							}
							_step = 5;
							_cycle = 2;
						}
						break;
					}
				case 5:
					{
						if (_cycle <= 0)
						{
							_item.InteractingPlayer = null;
							_item.Mode = 0;
							_step = 0;
							await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
						}
						break;
					}
				case 6:
					{
						if (_cycle <= 0)
						{
							if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
								return;

							_item.InteractingPlayer.SetRotation(_item.Rotation);
							_item.InteractingPlayer.GoalPosition = tile.PositionInFront(_item.Rotation);

							_item.Mode = 1;
							_step = 7;
							_cycle = 3;
							await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
						}
						break;
					}
				case 7:
					{
						if (_cycle <= 0)
						{
							_item.Mode = 0;
							_step = 0;
							_item.InteractingPlayer = null;
							await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
						}
						break;
					}
				case 8:
					{
						if (_cycle <= 0)
						{
							_item.Mode = 0;
							_step = 0;
							_item.InteractingPlayer = null;
							await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
						}
						break;
					}
			}

			_cycle--;
		}
    }
}
