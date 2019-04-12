using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AliasPro.Room.Gamemap.Pathfinding
{
    using AliasPro.API.Items.Models;
    using AliasPro.Items.Types;
    using Models.Entities;

    public static class PathFinder
    {
        private static readonly AstarPosition[] DIAG = new AstarPosition[]
        {
            new AstarPosition(-1, -1, 0),
            new AstarPosition(0, -1, 0),
            new AstarPosition(1, -1, 0),
            new AstarPosition(1, 0, 0),
            new AstarPosition(1, 1, 0),
            new AstarPosition(0, 1, 0),
            new AstarPosition(-1, 1, 0),
            new AstarPosition(-1, 0, 0)
        };

        private static readonly AstarPosition[] NONDIAG = new AstarPosition[]
        {
            new AstarPosition(0, -1, 0),
            new AstarPosition(1, 0, 0),
            new AstarPosition(0, 1, 0),
            new AstarPosition(-1, 0, 0)
        };

        private static Position GetBedPosition(IItem item, Position end)
        {
            Position newPos = new Position(item.Position.X, item.Position.Y, item.Position.Z);

            if ((item.Rotation % 4) != 0)
            {
                if (end.Y != newPos.Y)
                    newPos.Y++;
            }
            else
            {
                if (end.X != newPos.X)
                    newPos.X++;
            }

            return newPos;
        }

        public static IList<Position> FindPath(
            BaseEntity entity,
            RoomMap roomGrid,
            Position start,
            Position end)
        {
            if (!roomGrid.TryGetRoomTile(end.X, end.Y, out RoomTile roomTile)) return null;

            if (!roomTile.IsValidTile(entity, true)) return null;

            IItem topItem = roomTile.TopItem;
            if (topItem != null)
            {
                if (topItem.ItemData.InteractionType == ItemInteractionType.BED)
                {
                    end = GetBedPosition(topItem, end);
                }
            }

            BinaryHeap openHeap = new BinaryHeap();
            openHeap.Add(new HeapNode(start, ManhattanDistance(start, end)));

            float[,] currentCost = new float[roomGrid.MapSizeX, roomGrid.MapSizeY];
            Position[,] walkedPath = new Position[roomGrid.MapSizeX, roomGrid.MapSizeY];

            while (openHeap.HasEntry)
            {
                HeapNode curr = openHeap.Get();

                if (curr.Position == end)
                {
                    IList<Position> path = BuildPath(start, end, walkedPath, roomGrid.MapSizeX);

                    if (path.Count == 0) return null;
                    return path;
                }

                float cost = currentCost[curr.Position.X, curr.Position.Y];
                foreach (AstarPosition option in DIAG)
                {
                    Position position = curr.Position + option;

                    if (position != end &&
                        !IsValidStep(entity, roomGrid, position)) continue;

                    // Can't walk diagonal between two non-walkable tiles.
                    if (curr.Position.X != position.X &&
                            curr.Position.Y != position.Y)
                    {
                        bool firstValidTile = 
                            IsValidStep(entity, roomGrid, new Position(position.X, curr.Position.Y, 0));
                        bool secondValidTile = 
                            IsValidStep(entity, roomGrid, new Position(curr.Position.X, position.Y, 0));

                        if (!firstValidTile && !secondValidTile) continue;
                    }

                    float newCost = cost + option.Cost;
                    float oldCost = currentCost[position.X, position.Y];
                    if (!(oldCost <= 0) && !(newCost < oldCost))
                        continue;

                    currentCost[position.X, position.Y] = newCost;
                    walkedPath[position.X, position.Y] = curr.Position;

                    float expCost = newCost + ManhattanDistance(position, end);
                    openHeap.Add(new HeapNode(position, expCost));
                }
            }

            //No path was found.
            return null;
        }

        private static bool IsValidStep(BaseEntity entity, RoomMap roomGrid, Position position)
        {
            if (position.X >= roomGrid.MapSizeX || position.X < 0 || 
                position.Y >= roomGrid.MapSizeY || position.Y < 0)
                return false;
            
            if (roomGrid.TryGetRoomTile(position.X, position.Y, out RoomTile roomTile))
                return roomTile.IsValidTile(entity);

            return false;
        }

        private static IList<Position> BuildPath(
            Position start,
            Position end,
            Position[,] walkedPath,
            int dimensionX)
        {
            List<Position> path = new List<Position> { end };
            Position current = end;
            while (current != start)
            {
                Position prev = walkedPath[current.X, current.Y];
                current = prev;
                path.Add(current);
            }

            path.RemoveAt(path.Count - 1);
            return path;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float ManhattanDistance(Position start, Position end) =>
            Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetIndexUnchecked(int x, int y, int dimX) =>
            dimX * y + x;
    }
}
