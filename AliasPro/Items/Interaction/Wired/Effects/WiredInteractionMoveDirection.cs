using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionMoveDirection : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.MOVE_DIRECTION;

        private bool _active = false;
        private int _tick = 0;

        public WiredInteractionMoveDirection(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool Execute(params object[] args)
        {
            if (!_active)
            {
                _active = true;
                _tick = WiredData.Delay;
            }
            return true;
        }

        public async override void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    foreach (WiredItemData itemData in WiredData.Items.Values)
                    {
                        if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item)) continue;

                        if (itemData.MovementDirection == -1)
                        {
                            itemData.MovementDirection = StartDirection;
                            Item.ExtraData = WiredData.ToString();
                        }

                        IRoomPosition newPos = HandleMovement(itemData.MovementDirection, item.Position);

                        Room.Items.TriggerWired(WiredInteractionType.COLLISION, newPos);

                        if (!Room.RoomGrid.TryGetRoomTile(newPos.X, newPos.Y, out IRoomTile roomTile) ||
                            !Room.RoomGrid.CanRollAt(newPos.X, newPos.Y, item))
                        {
                            itemData.MovementDirection = HandleDirection(DirectionMode, itemData.MovementDirection);
                            Item.ExtraData = WiredData.ToString();
                            continue;
                        }
                        
                        newPos.Z = roomTile.Height;
                        await Room.SendPacketAsync(new FloorItemOnRollerComposer(item, newPos, 0));

                        Room.RoomGrid.RemoveItem(item);
                        item.Position = newPos;
                        Room.RoomGrid.AddItem(item);
                    }
                    _active = false;
                }
                _tick--;
            }
        }

        private IRoomPosition HandleMovement(int mode, IRoomPosition position)
        {
            IRoomPosition newPos =
                new RoomPosition(position.X, position.Y, position.Z);
            
            switch (mode)
            {
                case 0: newPos.Y--; break;
                case 2: newPos.X++; break;
                case 4: newPos.Y++; break;
                case 6: newPos.X--; break;
            }

            return newPos;
        }

        private int HandleDirection(int mode, int direction)
        {
            int newDirection = direction;
            int randomNum = Randomness.RandomNumber(1, 4);

            switch (mode)
            {
                case 1: newDirection += 2; break;
                case 2: newDirection += 4; break;
                case 3: newDirection -= 2; break;
                case 4: newDirection -= 4; break;
                case 5: newDirection += 4; break;
                case 6: newDirection += 4; break;
                case 7:
                    {
                        if (randomNum % 2 == 0)
                            newDirection += 2;
                        else
                            newDirection -= 2;
                        break;
                    }
            }

            if (newDirection > 6) newDirection = 0 + newDirection - 8;
            if (newDirection < 0) newDirection = 8 + newDirection;

            return newDirection;
        }

        private int StartDirection =>
            (WiredData.Params.Count <= 0) ? 0 : WiredData.Params[0];

        private int DirectionMode =>
            (WiredData.Params.Count <= 1) ? 0 : WiredData.Params[1];
    }
}
