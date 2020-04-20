using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Games;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Items.Interaction;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Games;
using AliasPro.Rooms.Games.Types;
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

            IRoom room = _interaction.Item.CurrentRoom;
            if (room == null)
                return;

            if (_currentStep >= _totalSteps)
            {
                Dead = true;
                _interaction.Item.ExtraData = "0";
                await room.SendPacketAsync(new FloorItemUpdateComposer(_interaction.Item));
                return;
            }

            if (!room.RoomGrid.TryGetRoomTile(_interaction.Item.Position.X, _interaction.Item.Position.Y, out IRoomTile currentTile))
                return;

            room.RoomGrid.TryGetTileInFront(currentTile.Position.X, currentTile.Position.Y, _currentDirection, out IRoomTile nextTile);



            if (nextTile != null && _interaction.ValidBounce(nextTile, _currentDirection))
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

                if (nextTile.TopItem != null && nextTile.TopItem.ItemData.InteractionType == ItemInteractionType.FOOTBALL_GOAL)
                {
                    if (!room.GameNew.TryGetGame(GameType.FOOTBALL, out BaseGame game))
                    {
                        game = new FootballGame(room);
                        room.GameNew.TryAddGame(game);
                    }

                    game.GivePlayerPoints(_entity, 1);

                    string colour = nextTile.TopItem.ItemData.Name.Split("fball_goal_")[1];

                    foreach (IItem scoreboard in room.Items.GetItemsByType(ItemInteractionType.FOOTBALL_SCOREBOARD))
                    {
                        if (scoreboard.ItemData.Name == "fball_score_" + colour)
                        {
                            int.TryParse(scoreboard.ExtraData, out int currentScore);
                            scoreboard.ExtraData = $"{currentScore + 1}";
                            await room.SendPacketAsync(new FloorItemUpdateComposer(scoreboard));
                        }
                    }
                }

                _interaction.Item.ExtraData = delay <= 200 ? "8" : (delay <= 250 ? "7" : (delay <= 300 ? "6" : (delay <= 350 ? "5" : (delay <= 400 ? "4" : (delay <= 450 ? "3" : (delay <= 500 ? "2" : "1"))))));
                await room.SendPacketAsync(new FloorItemUpdateComposer(_interaction.Item));

                IRoomPosition newPosition = new RoomPosition(nextTile.Position.X, nextTile.Position.Y, nextTile.Height);

                await room.SendPacketAsync(new FloorItemOnRollerComposer(_interaction.Item, newPosition, 0));

                room.RoomGrid.RemoveItem(_interaction.Item);
                _interaction.Item.Position = newPosition;
                room.RoomGrid.AddItem(_interaction.Item);

                await TaskManager.ExecuteTask(this, delay);
            }
        }
    }
}
