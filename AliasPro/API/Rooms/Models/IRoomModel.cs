namespace AliasPro.API.Rooms.Models
{
    public interface IRoomModel
    {
        int MapSizeX { get; set; }
        int MapSizeY { get; set; }
        int DoorDir { get; set; }
        int DoorX { get; set; }
        int DoorY { get; set; }
        double DoorZ { get; set; }
        string Id { get; set; }
        string HeightMap { get; set; }
        string RelativeHeightMap { get; set; }
        double GetHeight(int x, int y);
        bool GetTileState(int x, int y);
    }
}
