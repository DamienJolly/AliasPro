using AliasPro.API.Rooms.Models;

namespace AliasPro.Rooms.Gamemap.Pathfinding
{
    internal class HeapNode
    {
        internal HeapNode(IRoomPosition position, float expectedCost)
        {
            Position = position;
            ExpectedCost = expectedCost;
        }
        
        public IRoomPosition Position { get; }
        public float ExpectedCost { get; set; }
        public HeapNode Next { get; set; }
    }
}
