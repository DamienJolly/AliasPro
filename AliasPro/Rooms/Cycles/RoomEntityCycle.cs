using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using Pathfinding;
using Pathfinding.Models;
using Pathfinding.Types;
using System.Collections.Generic;
using System.Text;

namespace AliasPro.Rooms.Cycles
{
    public class RoomEntityCycle
    {
        private readonly BaseEntity _entity;
        private readonly StringBuilder _moveStatus;

        public RoomEntityCycle(BaseEntity entity)
        {
            _entity = entity;
            _moveStatus = new StringBuilder();
        }

        public async void Cycle()
        {
            if (_entity.HeadRotation != _entity.BodyRotation)
            {
                _entity.DirOffsetTimer++;

                if (_entity.DirOffsetTimer >= 4)
                    _entity.SetRotation(_entity.BodyRotation);
            }

            if (_entity.Position != _entity.NextPosition)
            {
                _entity.Position = _entity.NextPosition;

                if (_entity.NextPosition.X == _entity.Room.RoomModel.DoorX &&
                    _entity.NextPosition.Y == _entity.Room.RoomModel.DoorY)
                    await _entity.Room.RemoveEntity(_entity);
            }

            if (_entity.Position.X != _entity.GoalPosition.X ||
				_entity.Position.Y != _entity.GoalPosition.Y)
            {
                _entity.Actions.RemoveStatus("mv");

                IList<Position> path = null;

                try
                {
                    path = Pathfinder.FindPath(
                        _entity.Room.RoomGrid,
                        new Position(_entity.Position.X, _entity.Position.Y),
                        new Position(_entity.GoalPosition.X, _entity.GoalPosition.Y),
                        DiagonalMovement.ONE_WALKABLE,
                        _entity);
                }
                catch { }

                if (path == null || path.Count <= 0)
				{
                    _entity.GoalPosition = _entity.Position;
					return;
				}

                Position nextStep = path[path.Count - 1];

                if (!_entity.Room.RoomGrid.TryGetRoomTile(nextStep.X, nextStep.Y, out IRoomTile nextTile))
				{
					_entity.GoalPosition = _entity.Position;
					return;
				}

                if (!nextTile.IsValidTile(_entity, path.Count <= 1))
				{
					_entity.GoalPosition = _entity.Position;
					return;
				}

                if (!_entity.Room.RoomGrid.TryGetRoomTile(_entity.Position.X, _entity.Position.Y, out IRoomTile oldTile))
					{
					_entity.GoalPosition = _entity.Position;
					return;
				}

                _entity.Actions.RemoveStatus("sit");
                _entity.Actions.RemoveStatus("lay");
                _entity.IsSitting = false;
                _entity.IsLaying = false;
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
                _entity.NextPosition = new RoomPosition(
                    nextTile.Position.X,
                    nextTile.Position.Y,
                    newZ);

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
                _entity.NeedsUpdate = true;
            }
            else
            {
                _entity.Actions.RemoveStatus("mv");

                if (!_entity.IsLaying)
                    _entity.Actions.RemoveStatus("lay");

                if (!_entity.IsSitting)
                    _entity.Actions.RemoveStatus("sit");

                if (_entity.IsKicked)
                    await _entity.Room.RemoveEntity(_entity);

                if (!_entity.Room.RoomGrid.TryGetRoomTile(_entity.Position.X, _entity.Position.Y, out IRoomTile roomTile))
                    return;

                IItem topItem = roomTile.TopItem;
                double newZ = roomTile.Height;

                if (topItem != null)
                {
                    if (topItem.ItemData.InteractionType == ItemInteractionType.CHAIR ||
                        topItem.ItemData.InteractionType == ItemInteractionType.BED)
                    {
                        string type = topItem.ItemData.InteractionType == ItemInteractionType.CHAIR ? "sit" : "lay";

                        _entity.Actions.AddStatus(type, topItem.ItemData.Height + "");
                        _entity.SetRotation(topItem.Rotation);
						newZ -= topItem.ItemData.Height;
                        _entity.IsSitting = false;
                        _entity.IsLaying = false;
                    }
				}

				_entity.Position.Z = newZ;
			}

            _entity.Cycle();
            _entity.NeedsUpdate = true;
        }
    }
}
