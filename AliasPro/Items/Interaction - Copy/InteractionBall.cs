using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Tasks;
using AliasPro.Rooms.Entities;
using AliasPro.Tasks;

namespace AliasPro.Items.Interaction
{
    public class InteractionBall : IItemInteractor
    {
        public readonly IItem Item;

        private KickBallTask _currentTask = null;

        public InteractionBall(IItem item)
        {
            Item = item;
        }

		public void Compose(ServerMessage message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(1);
			message.WriteInt(0);
            message.WriteString(Item.ExtraData);
        }

		public void OnPlaceItem()
		{

		}

		public void OnPickupItem()
		{

		}

		public void OnMoveItem()
		{

		}

		public async void OnUserWalkOn(BaseEntity entity)
        {
            int velocity;
            int direction;

            //Player clicked on the tile the ball is on, they want to kick it
            if (Item.Position.X == entity.GoalPosition.X && Item.Position.Y == entity.GoalPosition.Y)
            {
                velocity = GetWalkOnVelocity(entity);
                direction = entity.BodyRotation;
                entity.DribbleDuration = 0;
            }
            //Player is walking past the ball, they want to drag it with them
            else
            {
                velocity = GetDragVelocity(entity);
                direction = entity.BodyRotation;
                entity.DribbleDuration++;
            }

            if (velocity > 0)
            {
                if (_currentTask != null)
                    _currentTask.Dead = true;

                _currentTask = new KickBallTask(this, entity, velocity, direction);
                await TaskManager.ExecuteTask(_currentTask);
            }
        }

        public async void OnUserWalkOff(BaseEntity entity)
        {
            if (!(_currentTask == null || _currentTask.Dead))
                return;

            int velocity = GetWalkOffVelocity(entity);
            int direction = GetWalkOffRotation(entity.BodyRotation);


            if (velocity > 0)
            {
                if (_currentTask != null)
                    _currentTask.Dead = true;

                _currentTask = new KickBallTask(this, entity, velocity, direction);
                await TaskManager.ExecuteTask(_currentTask);
            }
        }

        public async void OnUserInteract(BaseEntity entity, int state)
        {
            if (entity == null) return;

            if (!Item.CurrentRoom.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile tile))
                return;

            if (!tile.TilesAdjecent(entity.Position))
                return;

            int velocity = GetTackleVelocity(entity);
            int direction = entity.BodyRotation;


            if (velocity > 0)
            {
                if (_currentTask != null)
                    _currentTask.Dead = true;

                _currentTask = new KickBallTask(this, entity, velocity, direction);
                await TaskManager.ExecuteTask(_currentTask);
            }
        }

        public void OnCycle()
        {

        }

        private int GetWalkOnVelocity(BaseEntity entity)
        {
            if (entity.DribbleDuration == 1)
                return 0;
            if (entity.DribbleDuration > 1)
                return 1;
            return 6;
        }

        private int GetDragVelocity(BaseEntity entity)
        {
            return 1;
        }

        private int GetWalkOffVelocity(BaseEntity entity)
        {
            return 6;
        }

        private int GetTackleVelocity(BaseEntity entity)
        {
            return 4;
        }

        public bool ValidMove(IRoomTile tragetPos)
        {
            return Item.CurrentRoom.RoomGrid.CanRollAt(tragetPos.Position.X, tragetPos.Position.Y, Item);
        }

        public bool ValidBounce(IRoomTile tragetPos, int ballDirection)
        {
            if (tragetPos.Entities.Count > 0)
                return false;

            if (tragetPos.TopItem == null || tragetPos.TopItem.ItemData.InteractionType != Types.ItemInteractionType.FOOTBALL_GOAL)
                return false;

            if (!CanScore(tragetPos.TopItem.Rotation, ballDirection))
                return false;

            return true;
        }

        public int GetNextRollDelay(int currentStep, int totalSteps)
        {
            int t = 2500;
            return (totalSteps == 1) ? 500 : (100 * (((t = (t / t) - 1) * t * t * t * t) + 1)) + (currentStep * 100);
        }

        private bool CanScore(int goalRotation, int ballDirection)
        {
            return !((goalRotation + 3) % 8 == ballDirection || (goalRotation + 4) % 8 == ballDirection || (goalRotation + 5) % 8 == ballDirection);
        }

        private int GetWalkOffRotation(int currentDirection)
        {
            switch (currentDirection)
            {
                default:
                case 0:
                    return 4;
                case 1:
                    return 5;
                case 2:
                    return 6;
                case 3:
                    return 7;
                case 4:
                    return 0;
                case 5:
                    return 1;
                case 6:
                    return 2;
                case 7:
                    return 3;
            }
        }

        public int GetBounceDirection(int currentDirection)
        {
            switch (currentDirection)
            {
                default:
                case 0:
                    return 4;

                case 1:
                    if (Item.CurrentRoom.RoomGrid.TryGetTileInFront(Item.Position.X, Item.Position.Y, 7, out IRoomTile _1) && ValidMove(_1))
                        return 7;
                    if (Item.CurrentRoom.RoomGrid.TryGetTileInFront(Item.Position.X, Item.Position.Y, 3, out IRoomTile _2) && ValidMove(_2))
                        return 3;
                    return 5;

                case 2:
                    return 6;

                case 3:
                    if (Item.CurrentRoom.RoomGrid.TryGetTileInFront(Item.Position.X, Item.Position.Y, 5, out IRoomTile _3) && ValidMove(_3))
                        return 5;
                    if (Item.CurrentRoom.RoomGrid.TryGetTileInFront(Item.Position.X, Item.Position.Y, 1, out IRoomTile _4) && ValidMove(_4))
                        return 1;
                    return 7;

                case 4:
                    return 0;

                case 5:
                    if (Item.CurrentRoom.RoomGrid.TryGetTileInFront(Item.Position.X, Item.Position.Y, 3, out IRoomTile _5) && ValidMove(_5))
                        return 3;
                    if (Item.CurrentRoom.RoomGrid.TryGetTileInFront(Item.Position.X, Item.Position.Y, 7, out IRoomTile _6) && ValidMove(_6))
                        return 7;
                    return 1;

                case 6:
                    return 2;

                case 7:
                    if (Item.CurrentRoom.RoomGrid.TryGetTileInFront(Item.Position.X, Item.Position.Y, 1, out IRoomTile _7) && ValidMove(_7))
                        return 1;
                    if (Item.CurrentRoom.RoomGrid.TryGetTileInFront(Item.Position.X, Item.Position.Y, 5, out IRoomTile _8) && ValidMove(_8))
                        return 5;
                    return 3;
            }
        }

    }
}
