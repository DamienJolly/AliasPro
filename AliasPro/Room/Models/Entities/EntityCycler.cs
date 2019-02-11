﻿using System.Text;

namespace AliasPro.Room.Models.Entities
{
    using AliasPro.Item.Models;
    using Gamemap;
    using Gamemap.Pathfinding;

    internal class EntityCycler
    {
        private readonly IRoom _room;
        private readonly StringBuilder _moveStatus;

        internal EntityCycler(IRoom room)
        {
            _room = room;
            _moveStatus = new StringBuilder();
        }

        internal void Cycle(BaseEntity entity)
        {
            if (entity.NextPosition != entity.Position)
            {
                entity.Position = entity.NextPosition;
            }

            if (entity.PathToWalk != null)
            {
                entity.ActiveStatuses.Remove("mv");
                entity.ActiveStatuses.Remove("sit");
                entity.ActiveStatuses.Remove("lay");

                int reversedIndex = entity.PathToWalk.Count - 1;
                Position nextStep = entity.PathToWalk[reversedIndex];
                entity.PathToWalk.RemoveAt(reversedIndex);
                
                RoomTile roomTile = _room.RoomMap.GetRoomTile(
                    nextStep.X,
                    nextStep.Y);

                if (!roomTile.IsValidTile(entity.PathToWalk.Count == 0))
                {
                    entity.PathToWalk = PathFinder.FindPath(
                        _room.RoomMap,
                        entity.Position, entity.PathToWalk[0]);
                    return;
                }
                
                IItem topItem = roomTile.TopItem;
                double newZ = roomTile.Height;
                int newDir = 
                    Position.CalculateDirection(entity.Position, nextStep);

                if (topItem != null)
                {
                    if (topItem.ItemData.CanLay ||
                        topItem.ItemData.CanSit)
                        newZ -= topItem.ItemData.Height;
                }

                entity.NextPosition = new Position(nextStep.X, nextStep.Y, newZ);
                entity.BodyRotation = newDir;

                _moveStatus
                    .Clear()
                    .Append(nextStep.X)
                    .Append(",")
                    .Append(nextStep.Y)
                    .Append(",")
                    .Append(newZ);
                entity.ActiveStatuses.Add("mv", _moveStatus.ToString());

                if (entity.PathToWalk.Count == 0)
                    entity.PathToWalk = null;
            }
            else
            {
                RoomTile roomTile = _room.RoomMap.GetRoomTile(
                    entity.Position.X, 
                    entity.Position.Y);

                IItem topItem = roomTile.TopItem;

                entity.ActiveStatuses.Remove("sit");
                entity.ActiveStatuses.Remove("lay");
                entity.ActiveStatuses.Remove("mv");

                if (topItem != null)
                {
                    if (topItem.ItemData.CanSit)
                    {
                        entity.ActiveStatuses.Add("sit", topItem.ItemData.Height + "");
                        entity.BodyRotation = topItem.Rotation;
                    }
                    else if (topItem.ItemData.CanLay)
                    {
                        entity.ActiveStatuses.Add("lay", topItem.ItemData.Height + "");
                        entity.BodyRotation = topItem.Rotation;
                    }
                }

                entity.Position.Z = roomTile.Height;
            }
        }
    }
}
