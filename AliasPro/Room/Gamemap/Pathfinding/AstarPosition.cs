using AliasPro.Rooms.Models;

namespace AliasPro.Rooms.Gamemap.Pathfinding
{
    internal class AstarPosition : RoomPosition
    {
        private const float DiagonalCost = 1.4142135623730950488016887242097f;
        private const float LateralCost = 1.0f;

        public float Cost { get; }

        internal AstarPosition(int x, int y, double z)
            : base(x, y, z)
        {
            Cost = (x != 0 && y != 0) ? DiagonalCost : LateralCost;
        }
    }
}
