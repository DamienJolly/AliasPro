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
    public class WiredInteractionMoveRotate: IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.MOVE_ROTATE;

        private bool _active = false;
        private int _tick = 0;

        public IWiredData WiredData { get; set; }

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
                        if (!_item.CurrentRoom.Items.TryGetItem(itemData.ItemId, out IItem item)) continue;

                        int movementMode = 0;
                        int rotationMode = 0;

                        if (WiredData.Params.Count >= 2)
                        {
                            movementMode = WiredData.Params[0];
                            rotationMode = WiredData.Params[1];
                        }

                        IRoomPosition newPos = HandleMovement(movementMode, item.Position);
                        int newRot = HandleRotation(rotationMode, item.Rotation);

                        _item.CurrentRoom.Items.TriggerWired(WiredInteractionType.COLLISION, newPos);

                        if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(newPos.X, newPos.Y, out IRoomTile roomTile) ||
                            !_item.CurrentRoom.RoomGrid.CanRollAt(newPos.X, newPos.Y, item))
                            continue;

                        newPos.Z = roomTile.Height;
                        await _item.CurrentRoom.SendAsync(new FloorItemOnRollerComposer(item, newPos, 0));
                        
                        _item.CurrentRoom.RoomGrid.RemoveItem(item);
                        item.Position = newPos;
                        item.Rotation = newRot;
                        _item.CurrentRoom.RoomGrid.AddItem(item);
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

        private IRoomPosition HandleMovement(int Mode, IRoomPosition position)
        {
            IRoomPosition newPos = 
                new RoomPosition(position.X, position.Y, position.Z);
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
