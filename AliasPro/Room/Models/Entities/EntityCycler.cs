using System.Text;

namespace AliasPro.Rooms.Models.Entities
{
    using AliasPro.API.Items.Models;
    using AliasPro.API.Rooms.Entities;
    using AliasPro.API.Rooms.Models;
    using AliasPro.Items.Types;
    using AliasPro.Rooms.Packets.Composers;
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
                    _room.Mapping,
                    entity.Position, entity.PathToWalk[0]);

                if (entity.PathToWalk == null) return;

                entity.Actions.RemoveStatus("mv");
                entity.Actions.RemoveStatus("sit");
                entity.Actions.RemoveStatus("lay");
                entity.IsSitting = false;

                int reversedIndex = entity.PathToWalk.Count - 1;
                IRoomPosition nextStep = entity.PathToWalk[reversedIndex];
                entity.PathToWalk.RemoveAt(reversedIndex);

                if (!_room.Mapping.TryGetRoomTile(nextStep.X, nextStep.Y, out IRoomTile roomTile))
                {
                    entity.PathToWalk = null;
                    return;
                }
                
                if (!roomTile.IsValidTile(entity, reversedIndex == 0))
                {
                    entity.PathToWalk = null;
                    return;
                }

                if (!_room.Mapping.TryGetRoomTile(entity.Position.X, entity.Position.Y, out IRoomTile oldTile))
                {
                    entity.PathToWalk = null;
                    return;
                }

                _room.Mapping.RemoveEntity(entity);
                
                IItem oldTopItem = oldTile.TopItem;
                if (oldTopItem != null)
                {
                    oldTopItem.Interaction.OnUserWalkOff(entity);
                    _room.Items.TriggerWired(WiredInteractionType.WALKS_OFF_FURNI, entity, oldTopItem);
                }

                IItem topItem = roomTile.TopItem;
                double newZ = roomTile.Height;
                int newDir = entity.Position.CalculateDirection(nextStep);

                if (topItem != null)
                {
                    if (topItem.ItemData.InteractionType == ItemInteractionType.BED ||
                        topItem.ItemData.InteractionType == ItemInteractionType.CHAIR)
                        newZ -= topItem.ItemData.Height;

                    topItem.Interaction.OnUserWalkOn(entity);
                    _room.Items.TriggerWired(WiredInteractionType.WALKS_ON_FURNI, entity, topItem);
                }

                entity.NextPosition = new RoomPosition(nextStep.X, nextStep.Y, newZ);
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

                _room.Mapping.AddEntity(entity);
                _room.Entities.Unidle(entity);

                if (entity.PathToWalk.Count == 0)
                {
                    entity.PathToWalk = null;
                    return;
                }
            }
            else
            {
                if (!_room.Mapping.TryGetRoomTile(entity.Position.X, entity.Position.Y, out IRoomTile roomTile)) return;
                
                IItem topItem = roomTile.TopItem;
                
                entity.Actions.RemoveStatus("mv");
                entity.Actions.RemoveStatus("lay");

                if (!entity.IsSitting)
                    entity.Actions.RemoveStatus("sit");

                double z = roomTile.Height;

                if (topItem != null)
                {
                    if (topItem.ItemData.InteractionType == ItemInteractionType.CHAIR)
                    {
                        entity.Actions.AddStatus("sit", topItem.ItemData.Height + "");
                        entity.BodyRotation = 
                            entity.HeadRotation = 
                            topItem.Rotation;
                        entity.Position.Z = roomTile.Height - topItem.ItemData.Height;
                        entity.IsSitting = false;
                    }
                    else if (topItem.ItemData.InteractionType == ItemInteractionType.BED)
                    {
                        entity.Actions.AddStatus("lay", topItem.ItemData.Height + "");
                        entity.BodyRotation = 
                            entity.HeadRotation = 
                            topItem.Rotation;
                        entity.Position.Z = roomTile.Height - topItem.ItemData.Height;
                        entity.IsSitting = false;
                    }
                }
            }

            if (entity.HeadRotation != entity.BodyRotation)
            {
                entity.DirOffsetTimer++;

                if (entity.DirOffsetTimer >= 4)
                    entity.HeadRotation = entity.BodyRotation;
            }

            if (entity.HandItemId != 0)
            {
                entity.HandItemTimer--;
                if (entity.HandItemTimer <= 0)
                {
                    entity.SetHandItem(0);
                    await _room.SendAsync(new UserHandItemComposer(
                        entity.Id, 
                        entity.HandItemId));
                }
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
