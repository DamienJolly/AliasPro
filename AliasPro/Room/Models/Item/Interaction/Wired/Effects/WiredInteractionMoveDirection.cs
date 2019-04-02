using AliasPro.Item.Models;
using AliasPro.Item.Packets.Outgoing;
using AliasPro.Room.Gamemap;
using AliasPro.Utilities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionMoveDirection: IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.MOVE_DIRECTION;

        private bool _active = false;
        private int _tick = 0;

        public WiredData WiredData { get; set; }

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
                        if (!_item.CurrentRoom.ItemHandler.TryGetItem(itemData.ItemId, out IItem item)) continue;
                        
                        if (itemData.MovementDirection == -1)
                        {
                            itemData.MovementDirection = StartDirection;
                            _item.ExtraData = WiredData.ToString();
                        }
                        
                        Position newPos = HandleMovement(itemData.MovementDirection, item.Position);

                        _item.CurrentRoom.ItemHandler.TriggerWired(WiredInteraction.COLLISION, newPos);

                        // todo: roller effect?
                        if (_item.CurrentRoom.RoomMap.TryGetRoomTile(newPos.X, newPos.Y, out RoomTile roomTile) &&
                            _item.CurrentRoom.RoomMap.CanRollAt(newPos.X, newPos.Y, item))
                        {
                            _item.CurrentRoom.RoomMap.RemoveItem(item);
                            item.Position = newPos;
                            item.Position.Z = roomTile.Height;
                            _item.CurrentRoom.RoomMap.AddItem(item);

                            await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(item));
                        }
                        else
                        {
                            itemData.MovementDirection = HandleDirection(DirectionMode, itemData.MovementDirection);
                            _item.ExtraData = WiredData.ToString();
                        }
                    }
                    _active = false;
                }
                _tick--;
            }
        }

        private Position HandleMovement(int mode, Position position)
        {
            Position newPos =
                new Position(position.X, position.Y, position.Z);
            
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
