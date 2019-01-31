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
    }
}
