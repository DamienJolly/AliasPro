using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Room.Gamemap;
using AliasPro.Utilities;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionMoveRotate: IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.MOVE_ROTATE;

        private bool _active = false;
        private int _tick = 0;

        public WiredData WiredData { get; set; }

        public WiredInteractionMoveRotate(IItem item)
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

                        int movementMode = 0;
                        int rotationMode = 0;

                        if (WiredData.Params.Count >= 2)
                        {
                            movementMode = WiredData.Params[0];
                            rotationMode = WiredData.Params[1];
                        }

                        Position newPos = HandleMovement(movementMode, item.Position);
                        int newRot = HandleRotation(rotationMode, item.Rotation);

                        _item.CurrentRoom.ItemHandler.TriggerWired(WiredInteraction.COLLISION, newPos);
                        
                        // todo: roller effect?
                        if (_item.CurrentRoom.RoomMap.TryGetRoomTile(newPos.X, newPos.Y, out RoomTile roomTile))
                        {
                            if (_item.CurrentRoom.RoomMap.CanRollAt(newPos.X, newPos.Y, item))
                            {
                                _item.CurrentRoom.RoomMap.RemoveItem(item);
                                item.Position = newPos;
                                item.Position.Z = roomTile.Height;
                                item.Rotation = newRot;
                                _item.CurrentRoom.RoomMap.AddItem(item);
                            }
                        }

                        await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(item));
                    }

                    _active = false;
                }
                _tick--;
            }
        }

        private int HandleRotation(int mode, int rotation)
        {
            int randomNum = Randomness.RandomNumber(1, 4);

            switch (mode)
            {
                case 1: rotation += 2; break;
                case 2: rotation -= 2; break;
                case 3:
                    {
                        if (randomNum % 2 == 0)
                            rotation += 2;
                        else
                            rotation -= 2;
                        break;
                    }
            }

            if (rotation > 6) rotation = 0;
            if (rotation < 0) rotation = 6;

            return rotation;
        }

        private Position HandleMovement(int Mode, Position position)
        {
            Position newPos = 
                new Position(position.X, position.Y, position.Z);
            int randomNum = Randomness.RandomNumber(1, 4);

            switch (Mode)
            {
                case 1:
                    {
                        switch (randomNum)
                        {
                            case 1: newPos.X += 1; break;
                            case 2: newPos.X -= 1; break;
                            case 3: newPos.Y += 1; break;
                            case 4: newPos.Y -= 1; break;
                        }
                        break;
                    }
                case 2:
                    {
                        if (randomNum % 2 == 0)
                            newPos.X += 1;
                        else
                            newPos.X -= 1;
                        break;
                    }
                case 3:
                    {
                        if (randomNum % 2 == 0)
                            newPos.Y += 1;
                        else
                            newPos.Y -= 1;
                        break;
                    }
                case 4: newPos.Y -= 1; break;
                case 5: newPos.X += 1; break;
                case 6: newPos.Y += 1; break;
                case 7: newPos.X -= 1; break;
            }

            return newPos;
        }
    }
}
