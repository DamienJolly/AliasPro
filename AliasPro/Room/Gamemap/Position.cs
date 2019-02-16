using System;

namespace AliasPro.Room.Gamemap
{
    public class Position : IEquatable<Position>
    {
        public Position(int x, int y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        public int X { get; set; }
        public int Y { get; set; }
        public double Z { get; set; }

        public override bool Equals(object other)
        {
            if (other is Position position)
            {
                return position.Equals(this);
            }

            return false;
        }

        public bool Equals(Position other)
        {
            return (X == other.X && Y == other.Y);
        }

        public static bool operator ==(Position first, Position compare)
        {
            return first.Equals(compare);
        }

        public static bool operator !=(Position first, Position compare)
        {
            return !first.Equals(compare);
        }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y, 0);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public int CalculateDirection(Position newPos) =>
            CalculateDirection(newPos.X, newPos.Y);

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
    }
}
