using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Room.Gamemap;
using AliasPro.Room.Gamemap.Pathfinding;
using AliasPro.Room.Models.Entities;
using AliasPro.Utilities;
using System.Collections.Generic;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionChase : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.CHASE;

        private bool _active = false;
        private int _tick = 0;

        public IWiredData WiredData { get; set; }

        public WiredInteractionChase(IItem item)
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
                        
                        Position newPos = HandlePosition(item.Position);
                        
                        _item.CurrentRoom.ItemHandler.TriggerWired(WiredInteractionType.COLLISION, newPos);
                        
                        // todo: roller effect?
                        if (_item.CurrentRoom.RoomMap.TryGetRoomTile(newPos.X, newPos.Y, out RoomTile roomTile))
                        {
                            if (_item.CurrentRoom.RoomMap.CanRollAt(newPos.X, newPos.Y, item))
                            {
                                _item.CurrentRoom.RoomMap.RemoveItem(item);
                                item.Position = newPos;
                                item.Position.Z = roomTile.Height;
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

        private Position HandlePosition(Position position)
        {
            Position newPos =
                new Position(position.X, position.Y, position.Z);
            IList<Position> closestPath = null;

            foreach (BaseEntity entity in _item.CurrentRoom.EntityHandler.Entities)
            {
                IList<Position> pathToItem = PathFinder.FindPath(
                    entity,
                    _item.CurrentRoom.RoomMap,
                    position, entity.Position);

                if (pathToItem == null) continue;

                if (closestPath == null || 
                    pathToItem.Count <= closestPath.Count)
                    closestPath = pathToItem;
            }
            
            if (closestPath != null && 
                closestPath.Count >= 1)
            {
                newPos = closestPath[closestPath.Count - 1];
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
