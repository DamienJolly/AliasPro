namespace AliasPro.API.Rooms.Models
{
    public interface IRoomPosition
    {
        int X { get; set; }
        int Y { get; set; }
        double Z { get; set; }
        
        int CalculateDirection(int newX, int newY);
        bool IsAdjecent(IRoomPosition targetPos);
        double Distance(IRoomPosition targetPos);
    }
}
