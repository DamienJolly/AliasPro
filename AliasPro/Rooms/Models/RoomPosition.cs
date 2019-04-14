using AliasPro.API.Rooms.Models;

namespace AliasPro.Rooms.Models
{
    internal class RoomPosition : IRoomPosition
    {
        internal RoomPosition(int x, int y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public double Z { get; set; }

        public int CalculateDirection(IRoomPosition newPos) =>
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
