using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionMoveDirection: IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.MOVE_DIRECTION;

        private bool _active = false;
        private int _tick = 0;

        public IWiredData WiredData { get; set; }

        public WiredInteractionMoveDirection(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }
        
        public bool OnTrigger(params object[] args)
        {
            if (!_active)
            {
                _active = true;
                _tick = WiredData.Delay;
            }
            return true;
        }

        public async void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    foreach (WiredItemData itemData in WiredData.Items.Values)
                    {
                        if (!_item.CurrentRoom.Items.TryGetItem(itemData.ItemId, out IItem item)) continue;

                        if (itemData.MovementDirection == -1)
                        {
                            itemData.MovementDirection = StartDirection;
                            _item.ExtraData = WiredData.ToString();
                        }

                        IRoomPosition newPos = HandleMovement(itemData.MovementDirection, item.Position);

                        _item.CurrentRoom.Items.TriggerWired(WiredInteractionType.COLLISION, newPos);

                        if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(newPos.X, newPos.Y, out IRoomTile roomTile) ||
                            !_item.CurrentRoom.RoomGrid.CanRollAt(newPos.X, newPos.Y, item))
                        {
                            itemData.MovementDirection = HandleDirection(DirectionMode, itemData.MovementDirection);
                            _item.ExtraData = WiredData.ToString();
                            continue;
                        }
                        
                        newPos.Z = roomTile.Height;
                        await _item.CurrentRoom.SendPacketAsync(new FloorItemOnRollerComposer(item, newPos, 0));

                        _item.CurrentRoom.RoomGrid.RemoveItem(item);
                        item.Position = newPos;
                        _item.CurrentRoom.RoomGrid.AddItem(item);
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
