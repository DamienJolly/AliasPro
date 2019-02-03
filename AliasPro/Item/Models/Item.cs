using System.Data.Common;

namespace AliasPro.Item.Models
{
    using Room.Gamemap;
    using Database;

    internal class Item : IItem
    {
        internal Item(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            ItemId = reader.ReadData<uint>("item_id");
            PlayerId = reader.ReadData<uint>("player_id");
            RoomId = reader.ReadData<uint>("room_id");
            Rotation = reader.ReadData<int>("rot");
            Mode = 0; // reader.ReadData<int>("mode");
            ExtraData = ""; // reader.ReadData<string>("extra_data");
            Position = new Position(
                reader.ReadData<int>("x"),
                reader.ReadData<int>("y"),
                reader.ReadData<double>("z"));
        }

        public uint Id { get; }
        public uint ItemId { get; }
        public uint PlayerId { get; }
        public uint RoomId { get; }
        public int Rotation { get; }
        public int Mode { get; }
        public string ExtraData { get; }
        public Position Position { get; }
        public IItemData ItemData { get; set; }
    }

    public interface IItem
    {
        uint Id { get; }
        uint ItemId { get; }
        uint PlayerId { get; }
        uint RoomId { get; }
        int Rotation { get; }
        int Mode { get; }
        string ExtraData { get; }
        Position Position { get; }
        IItemData ItemData { get; set; }
    }
}
