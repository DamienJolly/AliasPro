namespace AliasPro.Rooms.Gamemap.Pathfinding
{
    internal class BinaryHeap
    {
        private HeapNode head;

        internal bool HasEntry => head != null;

        internal void Add(HeapNode node)
        {
            if (head == null)
            {
                head = node;
            }
            else if (node.ExpectedCost < head.ExpectedCost)
            {
                node.Next = head;
                head = node;
            }
            else
            {
                HeapNode current = head;
                while (current.Next != null && current.Next.ExpectedCost <= node.ExpectedCost)
                {
                    current = current.Next;
                }

                node.Next = current.Next;
                current.Next = node;
            }
        }
        
        internal HeapNode Get()
        {
            HeapNode first = head;
            head = head.Next;

            return first;
        }
    }
}
