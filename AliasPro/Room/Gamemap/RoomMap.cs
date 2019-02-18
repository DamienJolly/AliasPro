using System.Collections.Generic;

namespace AliasPro.Room.Gamemap
{
    using AliasPro.Item.Models;
    using Models;
    using Models.Entities;

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
                    //todo: tile states
                    WalkableGrid[x, y] = roomModel.GetTileState(x, y);

                    double posZ = roomModel.GetHeight(x, y);
                    Position position = new Position(x, y, posZ);

                    _roomTiles.Add(ConvertTo1D(x, y), new RoomTile(room, position));
                }
            }
        }

        public bool CanStackAt(int targertX, int targetY, IItem item)
        {
            bool canStack = true;
            IList<RoomTile> tiles =
                GetTilesFromItem(targertX, targetY, item);

            foreach (RoomTile tile in tiles)
            {
                if (!tile.CanStack(item))
                {
                    canStack = false;
                }
            }
            return canStack;
        }

        public IList<RoomTile> GetTilesFromItem(int targetX, int targetY, IItem item)
        {
            IList<RoomTile> tiles = new List<RoomTile>();
            for (int x = targetX; x <= targetX + (item.Rotation == 0 || item.Rotation == 4 ? item.ItemData.Width : item.ItemData.Length) - 1; x++)
            {
                for (int y = targetY; y <= targetY + (item.Rotation == 0 || item.Rotation == 4 ? item.ItemData.Length : item.ItemData.Width) - 1; y++)
                {
                    tiles.Add(GetRoomTile(x, y));
                }
            }
            return tiles;
        }

        public void AddItem(IItem item)
        {
            IList<RoomTile> tiles = 
                GetTilesFromItem(item.Position.X, item.Position.Y, item);

            foreach (RoomTile tile in tiles)
            {
                tile.AddItem(item);
            }
        }

        public void RemoveItem(IItem item)
        {
            IList<RoomTile> tiles =
                GetTilesFromItem(item.Position.X, item.Position.Y, item);

            foreach (RoomTile tile in tiles)
            {
                tile.RemoveItem(item.Id);
            }
        }

        public void AddEntity(BaseEntity entity)
        {
            RoomTile tiles =
                GetRoomTile(entity.NextPosition.X, entity.NextPosition.Y);

            tiles.AddEntity(entity);
        }

        public void RemoveEntity(BaseEntity entity)
        {
            RoomTile tiles =
                GetRoomTile(entity.Position.X, entity.Position.Y);

            tiles.RemoveEntity(entity.Id);
        }
        
        public bool TryGetRoomTile(int x, int y, out RoomTile roomTile) =>
            _roomTiles.TryGetValue(ConvertTo1D(x, y), out roomTile);

        public RoomTile GetRoomTile(int x, int y) =>
            _roomTiles[ConvertTo1D(x, y)];
        
        internal int ConvertTo1D(int x, int y) => MapSizeX * y + x;
    }
}
