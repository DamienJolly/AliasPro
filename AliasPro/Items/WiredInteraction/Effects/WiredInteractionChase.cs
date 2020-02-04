﻿using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;
using Pathfinding;
using Pathfinding.Models;
using Pathfinding.Types;
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
                        if (!_item.CurrentRoom.Items.TryGetItem(itemData.ItemId, out IItem item)) continue;

                        IRoomPosition newPos = HandlePosition(item.Position);
                        
                        _item.CurrentRoom.Items.TriggerWired(WiredInteractionType.COLLISION, newPos);

                        if (!_item.CurrentRoom.RoomGrid.TryGetRoomTile(newPos.X, newPos.Y, out IRoomTile roomTile) ||
                            !_item.CurrentRoom.RoomGrid.CanRollAt(newPos.X, newPos.Y, item))
                            continue;

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

        private IRoomPosition HandlePosition(IRoomPosition position)
        {
            IRoomPosition newPos;
            
            if (!TryGetClosestPosition(position, out newPos))
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

        private bool TryGetClosestPosition(IRoomPosition position, out IRoomPosition newPosition)
        {
            newPosition = new RoomPosition(position.X, position.Y, position.Z);
            IList<Position> closestPath = null;
            
            foreach (BaseEntity entity in _item.CurrentRoom.Entities.Entities)
            {
                IList<Position> pathToItem = Pathfinder.FindPath(
                    _item.CurrentRoom.RoomGrid,
                    new Position(entity.Position.X, entity.Position.Y),
                    new Position(position.X, position.Y),
                    DiagonalMovement.ONE_WALKABLE);

                if (pathToItem.Count <= closestPath.Count)
                    closestPath = pathToItem;
            }

            if (closestPath != null)
                newPosition = new RoomPosition(closestPath[closestPath.Count - 1]);

            return closestPath == null ? false : true;
        }
    }
}
