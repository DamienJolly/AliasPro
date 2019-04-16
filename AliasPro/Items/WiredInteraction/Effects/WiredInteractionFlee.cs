using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionFlee : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.FLEE;

        private bool _active = false;
        private int _tick = 0;

        public IWiredData WiredData { get; set; }

        public WiredInteractionFlee(IItem item)
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

                        IRoomPosition newPos = HandlePosition(item.Position);
                        
                        _item.CurrentRoom.Items.TriggerWired(WiredInteractionType.COLLISION, newPos);

                        if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(newPos.X, newPos.Y, out IRoomTile roomTile) ||
                            !_item.CurrentRoom.RoomGrid.CanRollAt(newPos.X, newPos.Y, item))
                            continue;

                        newPos.Z = roomTile.Height;
                        await _item.CurrentRoom.SendAsync(new FloorItemOnRollerComposer(item, newPos, 0));

                        _item.CurrentRoom.RoomGrid.RemoveItem(item);
                        item.Position = newPos;
                        _item.CurrentRoom.RoomGrid.AddItem(item);
                    }

                    _active = false;
                }
                _tick--;
            }
        }

        private IRoomPosition HandlePosition(IRoomPosition position)
        {
            IRoomPosition newPos =
                new RoomPosition(position.X, position.Y, position.Z);

            BaseEntity target = null;
            double shortest = 100.0D;

            foreach (BaseEntity entity in _item.CurrentRoom.Entities.Entities)
            {
                double distance = _item.CurrentRoom.RoomGrid.Distance(entity.Position, position);
                if (distance <= shortest)
                {
                    target = entity;
                    shortest = distance;
                }
            }

            if (target != null)
            {
                if (target.Position.X == position.X)
                {
                    if (position.Y < target.Position.Y) newPos.Y--;
                    else newPos.Y++;
                }
                else if (target.Position.Y == position.Y)
                {
                    if (position.X < target.Position.X) newPos.X--;
                    else newPos.X++;
                }
                else if (target.Position.X - position.X > target.Position.Y - position.Y)
                {
                    if (target.Position.X - position.X > 0) newPos.X--;
                    else newPos.X++;
                }
                else
                {
                    if (target.Position.Y - position.Y > 0) newPos.Y--;
                    else newPos.Y++;
                }
            } 
            else
            {
                int randomNum = Randomness.RandomNumber(1, 4);
                switch (randomNum)
                {
                    case 1: newPos.X += 1; break;
                    case 2: newPos.X -= 1; break;
                    case 3: newPos.Y += 1; break;
                    case 4: newPos.Y -= 1; break;
                }
            }

            return newPos;
        }
    }
}
