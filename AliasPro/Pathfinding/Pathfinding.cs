using AliasPro.Pathfinding.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AliasPro.Pathfinding
{
    public class Pathfinding
    {
        public static IList<Position> FindPath(
            Position start,
            Position end)
        {
            List<Position> path = new List<Position>();
            Position[,] walkedPath = BuildPath(start, end);
            Position current = end;

            while (!(current.X == start.X && current.Y == start.Y))
            {
                Position prev = walkedPath[current.X, current.Y];
                current = prev;
                path.Add(current);
            }
            
            return path;
        }

        private static Position[,] BuildPath(Position start, Position end)
        {
            BinaryHeap openHeap = new BinaryHeap();
            openHeap.Add(new HeapNode(start, ManhattanDistance(start, end)));

            float[,] currentCost = new float[roomGrid.MapSizeX, roomGrid.MapSizeY];
            Position[,] walkedPath = new Position[roomGrid.MapSizeX, roomGrid.MapSizeY];

            while (openHeap.HasEntry)
            {
                HeapNode curr = openHeap.Get();

                float cost = currentCost[curr.Position.X, curr.Position.Y];
                foreach (AstarPosition option in DIAG) // todo: option
                {
                    Position position = new Position(
                        curr.Position.X + option.X,
                        curr.Position.Y + option.Y, 0);

                    // todo: valid step
                    //if (!validStep) continue;

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

            return walkedPath;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float ManhattanDistance(Position start, Position end) =>
            Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
    }
}
