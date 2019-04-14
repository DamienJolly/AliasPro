namespace AliasPro.Pathfinding.Models
{
    public class Position
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
    }
}
