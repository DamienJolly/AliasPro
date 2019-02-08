using System.Collections.Generic;

namespace AliasPro.Room.Gamemap
{
    using Models;

    public class RoomMap
    {
        public int MapSizeX { get; }
        public int MapSizeY { get; }
        public bool[,] WalkableGrid { get; }

        private readonly IDictionary<int, RoomTile> _roomTiles;

        public RoomMap(IRoom room, IRoomModel roomModel)
        {
            MapSizeX = roomModel.MapSizeX;
            MapSizeY = roomModel.MapSizeY;

            _roomTiles = new Dictionary<int, RoomTile>();
            WalkableGrid = new bool[MapSizeX, MapSizeY];
            for (int y = 0; y < MapSizeY; y++)
            {
                for (int x = 0; x < MapSizeX; x++)
                {
                    WalkableGrid[x, y] = roomModel.GetTileState(x, y);
                    _roomTiles.Add(ConvertTo1D(x, y), new RoomTile(room));
                }
            }
        }

        public bool TryGetRoomTile(int x, int y, out RoomTile roomTile) =>
            _roomTiles.TryGetValue(ConvertTo1D(x, y), out roomTile);

        public RoomTile GetRoomTile(int x, int y) =>
            _roomTiles[ConvertTo1D(x, y)];
        
        internal int ConvertTo1D(int x, int y) => MapSizeX * y + x;
    }
}
