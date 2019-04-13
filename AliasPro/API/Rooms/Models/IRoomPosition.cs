using System;

namespace AliasPro.API.Rooms.Models
{
    public interface IRoomPosition
    {
        int X { get; set; }
        int Y { get; set; }
        double Z { get; set; }

        int CalculateDirection(IRoomPosition newPos);
        int CalculateDirection(int newX, int newY);
    }
}
