using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AliasPro.Room.Gamemap.Pathfinding
{
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

        public static IList<Position> FindPath(
            RoomMap roomGrid,
            Position start,
            Position end)
        {
            BinaryHeap openHeap = new BinaryHeap();
            openHeap.Add(new HeapNode(start, ManhattanDistance(start, end)));

            float[,] currentCost = new float[roomGrid.MapSizeX, roomGrid.MapSizeY];
            Position[,] walkedPath = new Position[roomGrid.MapSizeX, roomGrid.MapSizeY];

            while (openHeap.HasEntry)
            {
                HeapNode curr = openHeap.Get();

                if (curr.Position == end)
                {
                    return BuildPath(start, end, walkedPath, roomGrid.MapSizeX);
                }
                float cost = currentCost[curr.Position.X, curr.Position.Y];
                foreach (AstarPosition option in DIAG)
                {
                    try
                    {
                        Position position = curr.Position + option;

                        if (!roomGrid.WalkableGrid[position.X, position.Y])
                            continue;

                        float newCost = cost + option.Cost;
                        float oldCost = currentCost[position.X, position.Y];
                        if (!(oldCost <= 0) && !(newCost < oldCost))
                            continue;

                        currentCost[position.X, position.Y] = newCost;
                        walkedPath[position.X, position.Y] = curr.Position;

                        float expCost = newCost + ManhattanDistance(position, end);
                        openHeap.Add(new HeapNode(position, expCost));
                    }
                    catch
                    {
                    }
                }
            }

            //No path was found.
            return null;
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
