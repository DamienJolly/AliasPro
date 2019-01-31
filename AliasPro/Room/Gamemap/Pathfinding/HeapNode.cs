namespace AliasPro.Room.Gamemap.Pathfinding
{
    internal class HeapNode
    {
        internal HeapNode(Position position, float expectedCost)
        {
            Position = position;
            ExpectedCost = expectedCost;
        }
        
        public Position Position { get; }
        public float ExpectedCost { get; set; }
        public HeapNode Next { get; set; }
    }
}
