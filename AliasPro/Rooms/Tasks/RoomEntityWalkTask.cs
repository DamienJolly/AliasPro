﻿using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Items.Types;
using Pathfinding;
using Pathfinding.Models;
using Pathfinding.Types;
using System.Collections.Generic;
using System.Text;

namespace AliasPro.Rooms.Tasks
{
    public class RoomEntityWalkTask : ITask
    {
        private readonly BaseEntity _entity;
        private readonly StringBuilder _moveStatus;

        public RoomEntityWalkTask(BaseEntity entity)
        {
            _entity = entity;
            _moveStatus = new StringBuilder();
        }

        public void Run()
        {
            if (_entity.Position != _entity.NextPosition)
                _entity.Position = _entity.NextPosition;

            if (_entity.Position != _entity.GoalPosition)
            {
                IList<Position> path = Pathfinder.FindPath(
                    _entity.Room.RoomGrid,
                    new Position(_entity.Position.X, _entity.Position.Y),
                    new Position(_entity.GoalPosition.X, _entity.GoalPosition.Y),
                    DiagonalMovement.ONE_WALKABLE,
                    _entity);

                if (path == null) return;

                _entity.Actions.RemoveStatus("mv");
                _entity.Actions.RemoveStatus("sit");
                _entity.Actions.RemoveStatus("lay");
                _entity.IsSitting = false;

                Position nextStep = path[path.Count - 1];

                if (!_entity.Room.RoomGrid.TryGetRoomTile(nextStep.X, nextStep.Y, out IRoomTile nextTile))
                    return;

                if (!nextTile.IsValidTile(_entity, path.Count > 1))
                    return;

                if (!_entity.Room.RoomGrid.TryGetRoomTile(_entity.Position.X, _entity.Position.Y, out IRoomTile oldTile))
                    return;

                _entity.Room.RoomGrid.RemoveEntity(_entity);

                IItem nexTopItem = nextTile.TopItem;
                IItem oldTopItem = oldTile.TopItem;
                double newZ = nextTile.Height;
                int newDir = _entity.Position.CalculateDirection(nextStep.X, nextStep.Y);

                if (oldTopItem != null)
                {
                    oldTopItem.Interaction.OnUserWalkOff(_entity);
                    _entity.Room.Items.TriggerWired(WiredInteractionType.WALKS_OFF_FURNI, _entity, oldTopItem);
                }

                if (nexTopItem != null)
                {
                    if (nexTopItem.ItemData.InteractionType == ItemInteractionType.BED ||
                        nexTopItem.ItemData.InteractionType == ItemInteractionType.CHAIR)
                        newZ -= nexTopItem.ItemData.Height;

                    nexTopItem.Interaction.OnUserWalkOn(_entity);
                    _entity.Room.Items.TriggerWired(WiredInteractionType.WALKS_ON_FURNI, _entity, nexTopItem);
                }

                _entity.SetRotation(newDir);
                _entity.NextPosition = nextTile.Position;
                _entity.NextPosition.Z = newZ;

                _moveStatus
                    .Clear()
                    .Append(_entity.NextPosition.X)
                    .Append(",")
                    .Append(_entity.NextPosition.Y)
                    .Append(",")
                    .Append(_entity.NextPosition.Z);
                _entity.Actions.AddStatus("mv", _moveStatus.ToString());

                _entity.Room.RoomGrid.AddEntity(_entity);
                _entity.Unidle();
            }
            else
            {
                _entity.Actions.RemoveStatus("mv");
                _entity.Actions.RemoveStatus("lay");

                if (!_entity.IsSitting)
                    _entity.Actions.RemoveStatus("sit");

                if (!_entity.Room.RoomGrid.TryGetRoomTile(_entity.Position.X, _entity.Position.Y, out IRoomTile roomTile))
                    return;

                IItem topItem = roomTile.TopItem;
                double z = roomTile.Height;

                if (topItem != null)
                {
                    if (topItem.ItemData.InteractionType == ItemInteractionType.CHAIR ||
                        topItem.ItemData.InteractionType == ItemInteractionType.BED)
                    {
                        string type = topItem.ItemData.InteractionType == ItemInteractionType.CHAIR ? "sit" : "lay";

                        _entity.Actions.AddStatus(type, topItem.ItemData.Height + "");
                        _entity.SetRotation(topItem.Rotation);
                        _entity.Position.Z = roomTile.Height - topItem.ItemData.Height;
                        _entity.IsSitting = false;
                    }
                }
            }
        }
    }
}