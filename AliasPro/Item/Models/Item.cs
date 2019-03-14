using System.Data.Common;

namespace AliasPro.Item.Models
{
    using Network.Protocol;
    using Room.Gamemap;
    using Database;
    using Room.Models.Item.Interaction;
    using Room.Models.Entities;

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
            Mode = reader.ReadData<int>("mode");
            ExtraData = reader.ReadData<string>("extra_data");
            Position = new Position(
                reader.ReadData<int>("x"),
                reader.ReadData<int>("y"),
                reader.ReadData<double>("z"));
        }

        internal Item(uint itemId, uint playerId, string extraData, IItemData itemData)
        {
            Id = 0;
            ItemId = itemId;
            PlayerId = playerId;
            PlayerUsername = "Damien";
            RoomId = 0;
            Rotation = 0;
            Mode = 0;
            ExtraData = extraData;
            Position = new Position(0, 0, 0.00);
            ItemData = itemData;
        }

        public void ComposeFloorItem(ServerPacket message)
        {
            message.WriteInt(Id);
            message.WriteInt(ItemData.SpriteId);
            message.WriteInt(Position.X);
            message.WriteInt(Position.Y);
            message.WriteInt(Rotation);
            message.WriteString(string.Format("{0:0.00}", Position.Z.ToString()));
            message.WriteString("");
            message.WriteInt(1);
            Interaction.Compose(message, this);
            message.WriteInt(-1);
            message.WriteInt(ItemData.Modes > 0 ? 1 : 0);
            message.WriteInt(PlayerId);
        }

        public void ComposeWallItem(ServerPacket message)
        {
            message.WriteString(Id + "");
            message.WriteInt(ItemData.SpriteId);
            message.WriteString(ExtraData);
            message.WriteString(Mode + "");
            message.WriteInt(-1);
            message.WriteInt(ItemData.Modes > 1 ? 1 : 0);
            message.WriteInt(PlayerId);
        }

        public uint Id { get; set; }
        public uint ItemId { get; }
        public uint PlayerId { get; }
        public string PlayerUsername { get; }
        public uint RoomId { get; set; }
        public int Rotation { get; set; }
        public int Mode { get; set; }
        public string ExtraData { get; set; }
        public Position Position { get; set; }
        public IItemData ItemData { get; set; }
        public BaseEntity InteractingPlayer { get; set; }

        private IItemInteractor _interaction { get; set; }

        public IItemInteractor Interaction
        {
            get
            {
                if (_interaction == null)
                    _interaction = ItemInteractor.GetItemInteractor(ItemData.InteractionType);

                return _interaction;
            }
        }
    }

    public interface IItem
    {
        void ComposeFloorItem(ServerPacket serverPacket);
        void ComposeWallItem(ServerPacket serverPacket);

        uint Id { get; set; }
        uint ItemId { get; }
        uint PlayerId { get; }
        string PlayerUsername { get; }
        uint RoomId { get; set; }
        int Rotation { get; set; }
        int Mode { get; set; }
        string ExtraData { get; set; }
        Position Position { get; set; }
        IItemData ItemData { get; set; }
        BaseEntity InteractingPlayer { get; set; }

        IItemInteractor Interaction { get; }
    }
}
