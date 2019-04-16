using AliasPro.API.Rooms.Models;
using Pathfinding.Models;

namespace AliasPro.Rooms.Models
{
    public class RoomPosition : Position, IRoomPosition
    {
        public RoomPosition(int x, int y, double z)
            : base (x, y)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        public double Z { get; set; }
        
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
