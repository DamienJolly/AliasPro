using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Utilities;
using Pathfinding.Models;
using System.Collections.Generic;

namespace AliasPro.Rooms.Models
{
    public class RoomGrid : BaseGrid
    {
        private readonly IRoom _room;
        private readonly IDictionary<int, IRoomTile> _roomTiles;

        public RoomGrid(IRoom room)
            : base(room.RoomModel.MapSizeX, room.RoomModel.MapSizeY)
        {
            _room = room;
            _roomTiles = new Dictionary<int, IRoomTile>();

            for (int y = 0; y < MapSizeY; y++)
            {
                for (int x = 0; x < MapSizeX; x++)
                {
                    if (!_room.RoomModel.GetTileState(x, y)) continue;

                    double posZ = _room.RoomModel.GetHeight(x, y);
                    IRoomPosition position = new RoomPosition(x, y, posZ);

                    _roomTiles.Add(ConvertTo1D(x, y), new RoomTile(_room, position));
                }
            }
        }

        public override bool IsWalkableAt(Position p, bool final, params object[] args)
        {
            if (!TryGetRoomTile(p.X, p.Y, out IRoomTile tile))
                return false;

            BaseEntity entity = null;
            if (args.Length != 0)
                entity = (BaseEntity)args[0];

            return tile.IsValidTile(entity, final);
        }

        public bool CanStackAt(int targertX, int targetY, IItem item)
        {
            bool canStack = true;
            IList<IRoomTile> tiles =
                GetTilesFromItem(targertX, targetY, item);

            foreach (IRoomTile tile in tiles)
            {
                if (!tile.CanStack(item))
                {
                    canStack = false;
                }
            }
            return canStack;
        }

        public bool CanRollAt(int targertX, int targetY, IItem item)
        {
            bool canRoll = true;
            IList<IRoomTile> tiles =
                GetTilesFromItem(targertX, targetY, item);

            foreach (IRoomTile tile in tiles)
            {
                if (!tile.CanRoll(item))
                {
                    canRoll = false;
                }
            }
            return canRoll;
        }

        public IList<IRoomTile> GetTilesFromItem(int targetX, int targetY, IItem item)
        {
            IList<IRoomTile> tiles = new List<IRoomTile>();
            for (int x = targetX; x <= targetX + (item.Rotation == 0 || item.Rotation == 4 ? item.ItemData.Width : item.ItemData.Length) - 1; x++)
            {
                for (int y = targetY; y <= targetY + (item.Rotation == 0 || item.Rotation == 4 ? item.ItemData.Length : item.ItemData.Width) - 1; y++)
                {
                    if (TryGetRoomTile(x, y, out IRoomTile tile))
                        tiles.Add(tile);
                }
            }
            return tiles;
        }

		public bool TryGetRandomWalkableTile(out IRoomTile tile)
		{
			tile = null;
			for (int i = 0; i < 10; i++)
			{
				if (!TryGetRoomTile(Randomness.RandomNumber(MapSizeX - 1), Randomness.RandomNumber(MapSizeY - 1), out IRoomTile randomTile))
					continue;

				if (randomTile.IsValidTile(null, true))
				{
					tile = randomTile;
					return true;
				}
			}

			return false;
		}

		public bool TryGetTileInFrontOfItem(IItem item, out IRoomTile tile) =>
            TryGetTileInFront(item.Position.X, item.Position.Y, item.Rotation, out tile);

        public bool TryGetTileInFront(int x, int y, int rotation, out IRoomTile tile)
        {
            int posX = x;
            int posY = y;

            switch (rotation)
            {
                case 0: posY--; break;
                case 1: posX++; posY--; break;
                case 2: posX++; break;
                case 3: posX++; posY++; break;
                case 4: posY++; break;
                case 5: posX--; posY++; break;
                case 6: posX--; break;
                case 7: posX--; posY--; break;
            }

            return TryGetRoomTile(posX, posY, out tile);
        }
        
        public void AddItem(IItem item)
        {
            IList<IRoomTile> tiles =
                GetTilesFromItem(item.Position.X, item.Position.Y, item);

            foreach (IRoomTile tile in tiles)
            {
                tile.AddItem(item);
            }
        }

        public void RemoveItem(IItem item)
        {
            IList<IRoomTile> tiles =
                GetTilesFromItem(item.Position.X, item.Position.Y, item);

            foreach (IRoomTile tile in tiles)
            {
                tile.RemoveItem(item.Id);
            }
        }

        public void AddEntity(BaseEntity entity)
        {
            if (TryGetRoomTile(entity.NextPosition.X, entity.NextPosition.Y, out IRoomTile roomTile))
            {
                roomTile.AddEntity(entity);
            }
        }

        public void RemoveEntity(BaseEntity entity)
        {
            if (TryGetRoomTile(entity.Position.X, entity.Position.Y, out IRoomTile roomTile))
            {
                roomTile.RemoveEntity(entity.Id);
            }
        }

        public bool TryGetRoomTile(int x, int y, out IRoomTile roomTile) =>
            _roomTiles.TryGetValue(ConvertTo1D(x, y), out roomTile);

        internal int ConvertTo1D(int x, int y) => MapSizeX * y + x;
    }
}
