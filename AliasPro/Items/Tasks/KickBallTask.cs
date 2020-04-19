using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Tasks;

namespace AliasPro.Items.Tasks
{
	public class KickBallTask : ITask
	{
		public bool Dead = false;

		private readonly InteractionBall _interaction;
		private readonly BaseEntity _entity;
		private readonly int _totalSteps;

        private int _currentDirection;
        private int _currentStep = 0;

		public KickBallTask(InteractionBall interaction, BaseEntity entity, int totalSteps, int currentDirection)
		{
			_interaction = interaction;
			_entity = entity;
            _totalSteps = totalSteps;
            _currentDirection = currentDirection;
		}

		public async void Run()
		{
            if (Dead)
				return;

            if (_currentStep >= _totalSteps)
            {
                Dead = true;
                _interaction.Item.ExtraData = "0";
                await _interaction.Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_interaction.Item));
                return;
            }

            if (!_interaction.Item.CurrentRoom.RoomGrid.TryGetRoomTile(_interaction.Item.Position.X, _interaction.Item.Position.Y, out IRoomTile currentTile))
                return;

            _interaction.Item.CurrentRoom.RoomGrid.TryGetTileInFront(currentTile.Position.X, currentTile.Position.Y, _currentDirection, out IRoomTile nextTile);

            if (nextTile != null && nextTile.Entities.Count > 0) //back of goal
            {
                _currentStep = _totalSteps;
                await TaskManager.ExecuteTask(this);
                return;
            }

            _currentStep++;

            if (nextTile == null || !_interaction.ValidMove(nextTile))
            {
                int oldDirection = _currentDirection;
                _currentDirection = _interaction.GetBounceDirection(_currentDirection);

                if (_currentDirection == oldDirection)
                    _currentStep = _totalSteps;

                await TaskManager.ExecuteTask(this);
                return;
            }
            else
            {
                int delay = _interaction.GetNextRollDelay(_currentStep, _totalSteps);

                if (nextTile.TopItem != null && nextTile.TopItem.ItemData.Name.StartsWith("fball_goal_"))
                {
                    await _interaction.Item.CurrentRoom.SendPacketAsync(new UserActionComposer(_entity, 1));
                    //in goal
                }

                _interaction.Item.ExtraData = delay <= 200 ? "8" : (delay <= 250 ? "7" : (delay <= 300 ? "6" : (delay <= 350 ? "5" : (delay <= 400 ? "4" : (delay <= 450 ? "3" : (delay <= 500 ? "2" : "1"))))));
                await _interaction.Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_interaction.Item));

                IRoomPosition newPosition = new RoomPosition(nextTile.Position.X, nextTile.Position.Y, nextTile.Height);

                await _interaction.Item.CurrentRoom.SendPacketAsync(new FloorItemOnRollerComposer(_interaction.Item, newPosition, 0));

                _interaction.Item.CurrentRoom.RoomGrid.RemoveItem(_interaction.Item);
                _interaction.Item.Position = newPosition;
                _interaction.Item.CurrentRoom.RoomGrid.AddItem(_interaction.Item);

                await TaskManager.ExecuteTask(this, delay);
            }
        }
    }
}
