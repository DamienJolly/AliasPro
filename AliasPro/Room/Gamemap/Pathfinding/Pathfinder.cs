using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AliasPro.Rooms.Gamemap.Pathfinding
{
    using AliasPro.API.Items.Models;
    using AliasPro.API.Rooms.Entities;
    using AliasPro.API.Rooms.Models;
    using AliasPro.Items.Types;
    using AliasPro.Rooms.Components;
    using AliasPro.Rooms.Models;

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

        private static IRoomPosition GetBedPosition(IItem item, IRoomPosition end)
        {
            IRoomPosition newPos = new RoomPosition(item.Position.X, item.Position.Y, item.Position.Z);

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

        public static IList<IRoomPosition> FindPath(
            BaseEntity entity,
            MappingComponent roomGrid,
            IRoomPosition start,
            IRoomPosition end)
        {
            if (!roomGrid.TryGetRoomTile(end.X, end.Y, out IRoomTile roomTile)) return null;

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
            IRoomPosition[,] walkedPath = new RoomPosition[roomGrid.MapSizeX, roomGrid.MapSizeY];

            while (openHeap.HasEntry)
            {
                HeapNode curr = openHeap.Get();

                if (curr.Position.X == end.X && curr.Position.Y == end.Y)
                {
                    IList<IRoomPosition> path = BuildPath(start, end, walkedPath, roomGrid.MapSizeX);

                    if (path.Count == 0) return null;
                    return path;
                }

                float cost = currentCost[curr.Position.X, curr.Position.Y];
                foreach (AstarPosition option in DIAG)
                {
                    IRoomPosition position = new RoomPosition(
                        curr.Position.X + option.X,
                        curr.Position.Y + option.Y, 0);

                    if (!(position.X == end.X && position.Y == end.Y) &&
                        !IsValidStep(entity, roomGrid, position)) continue;
                    
                    // Can't walk diagonal between two non-walkable tiles.
                    if (!(curr.Position.X == position.X && curr.Position.Y == position.Y))
                    {
                        bool firstValidTile = 
                            IsValidStep(entity, roomGrid, new RoomPosition(position.X, curr.Position.Y, 0));
                        bool secondValidTile = 
                            IsValidStep(entity, roomGrid, new RoomPosition(curr.Position.X, position.Y, 0));

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

        private static bool IsValidStep(BaseEntity entity, MappingComponent roomGrid, IRoomPosition position)
        {
            if (roomGrid.TryGetRoomTile(position.X, position.Y, out IRoomTile roomTile))
                return roomTile.IsValidTile(entity);
            
            return false;
        }

        private static IList<IRoomPosition> BuildPath(
            IRoomPosition start,
            IRoomPosition end,
            IRoomPosition[,] walkedPath,
            int dimensionX)
        {
            List<IRoomPosition> path = new List<IRoomPosition> { end };
            IRoomPosition current = end;
            while (!(current.X == start.X && current.Y == start.Y))
            {
                IRoomPosition prev = walkedPath[current.X, current.Y];
                current = prev;
                path.Add(current);
            }

            path.RemoveAt(path.Count - 1);
            return path;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float ManhattanDistance(IRoomPosition start, IRoomPosition end) =>
            Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetIndexUnchecked(int x, int y, int dimX) =>
            dimX * y + x;
    }
}
