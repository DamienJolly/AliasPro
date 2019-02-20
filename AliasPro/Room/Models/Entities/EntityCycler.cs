using System.Text;

namespace AliasPro.Room.Models.Entities
{
    using AliasPro.Item.Models;
    using Gamemap;
    using Gamemap.Pathfinding;
    using Packets.Outgoing;

    internal class EntityCycler
    {
        private readonly IRoom _room;
        private readonly StringBuilder _moveStatus;

        internal EntityCycler(IRoom room)
        {
            _room = room;
            _moveStatus = new StringBuilder();
        }
        
        internal async void Cycle(BaseEntity entity)
        {
            if (entity.NextPosition != entity.Position)
            {
                entity.Position = entity.NextPosition;
            }

            if (entity.PathToWalk != null)
            {
                entity.PathToWalk = PathFinder.FindPath(
                    entity,
                    _room.RoomMap,
                    entity.Position, entity.PathToWalk[0]);

                if (entity.PathToWalk == null) return;

                entity.Actions.RemoveStatus("mv");
                entity.Actions.RemoveStatus("sit");
                entity.Actions.RemoveStatus("lay");
                entity.IsSitting = false;

                int reversedIndex = entity.PathToWalk.Count - 1;
                Position nextStep = entity.PathToWalk[reversedIndex];
                entity.PathToWalk.RemoveAt(reversedIndex);

                RoomTile roomTile = _room.RoomMap.GetRoomTile(
                    nextStep.X,
                    nextStep.Y);

                if (!roomTile.IsValidTile(entity, reversedIndex == 0))
                {
                    entity.PathToWalk = null;
                    return;
                }

                _room.RoomMap.RemoveEntity(entity);

                IItem topItem = roomTile.TopItem;
                double newZ = roomTile.Height;
                int newDir = entity.Position.CalculateDirection(nextStep);

                if (topItem != null)
                {
                    if (topItem.ItemData.CanLay ||
                        topItem.ItemData.CanSit)
                        newZ -= topItem.ItemData.Height;
                }
                
                entity.NextPosition = new Position(nextStep.X, nextStep.Y, newZ);
                entity.BodyRotation = newDir;
                entity.HeadRotation = newDir;

                _moveStatus
                    .Clear()
                    .Append(nextStep.X)
                    .Append(",")
                    .Append(nextStep.Y)
                    .Append(",")
                    .Append(newZ);
                entity.Actions.AddStatus("mv", _moveStatus.ToString());

                _room.RoomMap.AddEntity(entity);
                _room.EntityHandler.Unidle(entity);

                if (entity.PathToWalk.Count == 0)
                {
                    entity.PathToWalk = null;
                    return;
                }
            }
            else
            {
                RoomTile roomTile = _room.RoomMap.GetRoomTile(
                    entity.Position.X, 
                    entity.Position.Y);

                IItem topItem = roomTile.TopItem;
                
                entity.Actions.RemoveStatus("mv");
                entity.Actions.RemoveStatus("lay");

                if (!entity.IsSitting)
                    entity.Actions.RemoveStatus("sit");

                if (topItem != null)
                {
                    if (topItem.ItemData.CanSit)
                    {
                        entity.Actions.AddStatus("sit", topItem.ItemData.Height + "");
                        entity.BodyRotation = 
                            entity.HeadRotation = 
                            topItem.Rotation;
                        entity.IsSitting = false;
                    }
                    else if (topItem.ItemData.CanLay)
                    {
                        entity.Actions.AddStatus("lay", topItem.ItemData.Height + "");
                        entity.BodyRotation = 
                            entity.HeadRotation = 
                            topItem.Rotation;
                        entity.IsSitting = false;
                    }
                }

                entity.Position.Z = roomTile.Height;
            }

            if (entity.HeadRotation != entity.BodyRotation)
            {
                entity.DirOffsetTimer++;

                if (entity.DirOffsetTimer >= 4)
                    entity.HeadRotation = entity.BodyRotation;
            }

            entity.IdleTimer++;
            if (entity.IdleTimer >= 600 && !entity.IsIdle)
            {
                entity.IdleTimer = 0;
                entity.IsIdle = true;
                await _room.SendAsync(new UserSleepComposer(entity));
            }

            if (entity.IdleTimer >= 1800 && entity.IsIdle)
            {
                //todo: kickuser
            }
        }
    }
}
