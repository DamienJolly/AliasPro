using System.Text;

namespace AliasPro.Room.Models.Entities
{
    using AliasPro.Room.Models.Item;
    using Gamemap;

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
                //Finished walking or found no path.
                if (entity.PathToWalk.Count == 0)
                {
                    entity.ActiveStatuses.Remove("mv");
                    entity.PathToWalk = null;
                    return;
                }

                entity.ActiveStatuses.Remove("mv");

                int reversedIndex = entity.PathToWalk.Count - 1;
                Position nextStep = entity.PathToWalk[reversedIndex];
                entity.PathToWalk.RemoveAt(reversedIndex);
                
                IRoomItem topItem = null;
                if (_room.RoomMap.TryGetRoomTile(nextStep.X, nextStep.Y, out RoomTile roomTile))
                {
                    topItem = roomTile.GetTopItem();
                }
                
                double newZ = topItem == null ? 0.00 : (topItem.Position.Z + topItem.ItemData.Height);
                int newDir = Position.CalculateDirection(entity.Position, nextStep);

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
            }
            else
            {
                if (entity.ActiveStatuses.ContainsKey("mv"))
                {
                    entity.ActiveStatuses.Remove("mv");
                }
            }
        }
    }
}
