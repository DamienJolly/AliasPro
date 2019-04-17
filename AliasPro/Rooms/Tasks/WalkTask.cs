using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Tasks;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using Pathfinding.Models;
using System.Text;

namespace AliasPro.Rooms.Tasks
{
    public class WalkTask : ITask
    {
        private readonly IRoom _room;
        private readonly BaseEntity _entity;
        private readonly StringBuilder _moveStatus;

        public WalkTask(IRoom room, BaseEntity entity)
        {
            _room = room;
            _entity = entity;
            _moveStatus = new StringBuilder();
        }

        public void Run()
        {
            if (_entity.NextPosition != _entity.Position)
            {
                _entity.Position = _entity.NextPosition;
            }

            if (_entity.PathToWalk != null)
            {
                _entity.FindPath(
                    _entity.PathToWalk[0].X, 
                    _entity.PathToWalk[0].Y);

                if (_entity.PathToWalk == null) return;

                _entity.Actions.RemoveStatus("mv");
                _entity.Actions.RemoveStatus("sit");
                _entity.Actions.RemoveStatus("lay");
                _entity.IsSitting = false;

                int reversedIndex = _entity.PathToWalk.Count - 1;
                Position nextStep = _entity.PathToWalk[reversedIndex];
                _entity.PathToWalk.RemoveAt(reversedIndex);

                if (!_room.RoomGrid.TryGetRoomTile(nextStep.X, nextStep.Y, out IRoomTile roomTile))
                {
                    _entity.PathToWalk = null;
                    return;
                }

                if (!roomTile.IsValidTile(_entity, reversedIndex == 0))
                {
                    _entity.PathToWalk = null;
                    return;
                }

                if (!_room.RoomGrid.TryGetRoomTile(_entity.Position.X, _entity.Position.Y, out IRoomTile oldTile))
                {
                    _entity.PathToWalk = null;
                    return;
                }

                _room.RoomGrid.RemoveEntity(_entity);

                IItem oldTopItem = oldTile.TopItem;
                if (oldTopItem != null)
                {
                    oldTopItem.Interaction.OnUserWalkOff(_entity);
                    _room.Items.TriggerWired(WiredInteractionType.WALKS_OFF_FURNI, _entity, oldTopItem);
                }

                IItem topItem = roomTile.TopItem;
                double newZ = roomTile.Height;
                int newDir = _entity.Position.CalculateDirection(nextStep.X, nextStep.Y);

                if (topItem != null)
                {
                    if (topItem.ItemData.InteractionType == ItemInteractionType.BED ||
                        topItem.ItemData.InteractionType == ItemInteractionType.CHAIR)
                        newZ -= topItem.ItemData.Height;

                    topItem.Interaction.OnUserWalkOn(_entity);
                    _room.Items.TriggerWired(WiredInteractionType.WALKS_ON_FURNI, _entity, topItem);
                }

                _entity.NextPosition = new RoomPosition(nextStep.X, nextStep.Y, newZ);
                _entity.SetRotation(newDir);

                _moveStatus
                    .Clear()
                    .Append(nextStep.X)
                    .Append(",")
                    .Append(nextStep.Y)
                    .Append(",")
                    .Append(newZ);
                _entity.Actions.AddStatus("mv", _moveStatus.ToString());

                _room.RoomGrid.AddEntity(_entity);
                _entity.Unidle();

                if (_entity.PathToWalk.Count == 0)
                {
                    _entity.PathToWalk = null;
                    return;
                }
            }
            else
            {
                if (!_room.RoomGrid.TryGetRoomTile(_entity.Position.X, _entity.Position.Y, out IRoomTile roomTile)) return;

                IItem topItem = roomTile.TopItem;

                _entity.Actions.RemoveStatus("mv");
                _entity.Actions.RemoveStatus("lay");

                if (!_entity.IsSitting)
                    _entity.Actions.RemoveStatus("sit");

                double z = roomTile.Height;

                if (topItem != null)
                {
                    if (topItem.ItemData.InteractionType == ItemInteractionType.CHAIR)
                    {
                        _entity.Actions.AddStatus("sit", topItem.ItemData.Height + "");
                        _entity.SetRotation(topItem.Rotation);
                        _entity.Position.Z = roomTile.Height - topItem.ItemData.Height;
                        _entity.IsSitting = false;
                    }
                    else if (topItem.ItemData.InteractionType == ItemInteractionType.BED)
                    {
                        _entity.Actions.AddStatus("lay", topItem.ItemData.Height + "");
                        _entity.SetRotation(topItem.Rotation);
                        _entity.Position.Z = roomTile.Height - topItem.ItemData.Height;
                        _entity.IsSitting = false;
                    }
                }
            }

            //todo: make task
            if (_entity.HeadRotation != _entity.BodyRotation)
            {
                _entity.DirOffsetTimer++;

                if (_entity.DirOffsetTimer >= 4)
                    _entity.SetRotation(_entity.BodyRotation, true);
            }
        }
    }
}
