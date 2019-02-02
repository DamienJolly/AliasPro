using System.Data.Common;

namespace AliasPro.Room.Models.Item
{
    using Database;
    using AliasPro.Item.Models;
    using Gamemap;

    internal class RoomItem : IRoomItem
    {
        internal RoomItem(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            ItemId = reader.ReadData<uint>("item_id");
            Rotation = reader.ReadData<int>("rot");
            Position = new Position(
                reader.ReadData<int>("x"),
                reader.ReadData<int>("y"),
                reader.ReadData<double>("z"));
            Mode = 1;
        }

        public uint Id { get; }
        public uint ItemId { get; }
        public int Rotation { get; }
        public Position Position { get; }
        public int Mode { get; }
    }

    public interface IRoomItem
    {
        uint Id { get; }
        uint ItemId { get; }
        int Rotation { get; }
        Position Position { get; }
        int Mode { get; }
    }
}
