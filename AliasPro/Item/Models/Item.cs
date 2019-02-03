using System.Data.Common;

namespace AliasPro.Item.Models
{
    using Network.Protocol;
    using Room.Gamemap;
    using Database;

    internal class Item : IItem
    {
        internal Item(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            ItemId = reader.ReadData<uint>("item_id");
            PlayerId = reader.ReadData<uint>("player_id");
            PlayerUsername = "Damien";
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
        public string PlayerUsername { get; }
        public uint RoomId { get; set; }
        public int Rotation { get; set; }
        public int Mode { get; }
        public string ExtraData { get; }
        public Position Position { get; set; }
        public IItemData ItemData { get; set; }

        public void Compose(ServerPacket message)
        {
            message.WriteInt(Id);
            message.WriteInt(ItemData.SpriteId);
            message.WriteInt(Position.X);
            message.WriteInt(Position.Y);
            message.WriteInt(Rotation);
            message.WriteString(string.Format("{0:0.00}", Position.Z.ToString()));
            message.WriteString("");

            message.WriteInt(1);
            message.WriteInt(0);
            message.WriteString(ExtraData);

            message.WriteInt(-1);
            message.WriteInt(ItemData.Modes > 0 ? 1 : 0);
            message.WriteInt(PlayerId);
        }
    }

    public interface IItem
    {
        uint Id { get; }
        uint ItemId { get; }
        uint PlayerId { get; }
        string PlayerUsername { get; }
        uint RoomId { get; set; }
        int Rotation { get; set; }
        int Mode { get; }
        string ExtraData { get; }
        Position Position { get; set; }
        IItemData ItemData { get; set; }

        void Compose(ServerPacket serverPacket);
    }
}
