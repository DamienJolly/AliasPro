using AliasPro.API.Rooms.Models;
using Pathfinding.Models;
using System;

namespace AliasPro.Rooms.Models
{
    public class RoomPosition : Position, IEquatable<RoomPosition>, IRoomPosition
    {
        public double Z { get; set; }

        public RoomPosition(int x, int y, double z)
            : base (x, y)
        {
            Z = z;
        }

        public RoomPosition(Position pos)
            : base (pos.X, pos.Y)
        {
            Z = 0;
        }
        public override bool Equals(object other)
        {
            if (other is RoomPosition position)
            {
                return position.Equals(this);
            }

            return false;
        }

        public bool Equals(RoomPosition other)
        {
            return (X == other.X && Y == other.Y);
        }

        public static bool operator ==(RoomPosition first, RoomPosition compare)
        {
            return first.Equals(compare);
        }

        public static bool operator !=(RoomPosition first, RoomPosition compare)
        {
            return !first.Equals(compare);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public int CalculateDirection(int newX, int newY)
        {
            if (X > newX)
            {
                if (Y == newY)
                    return 6;
                else if (Y < newY)
                    return 5;
                else
                    return 7;
            }
            else if (X < newX)
            {
                if (Y == newY)
                    return 2;
                else if (Y < newY)
                    return 3;
                else
                    return 1;
            }
            else
            {
                if (Y < newY)
                    return 4;
                else
                    return 0;
            }
        }
        
        public bool IsAdjecent(IRoomPosition targetPos) =>
            Distance(targetPos) <= 1;

        public double Distance(IRoomPosition targetPos) =>
            Math.Sqrt(((X - targetPos.X) * (X - targetPos.X)) + ((Y - targetPos.Y) * (Y - targetPos.Y)));
    }
}
