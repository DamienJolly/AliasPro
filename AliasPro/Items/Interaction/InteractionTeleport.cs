using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Tasks;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Entities;
using AliasPro.Tasks;

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

			if (_item.ItemData.CanWalk) return;

			if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(_item.Position.X, _item.Position.Y, out IRoomTile tile))
				return;

			IRoomPosition position = tile.PositionInFront(_item.Rotation);
			if (!(position.X == entity.Position.X && position.Y == entity.Position.Y))
			{
				entity.GoalPosition = position;
				return;
			}

			_item.ItemData.CanWalk = true;
			_item.Mode = 1;
			entity.GoalPosition = _item.Position;
			await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
			await TaskManager.ExecuteTask(new TeleportTaskOne(_item, entity), 1500);
		}

        public void OnCycle()
        {

		}
    }
}
