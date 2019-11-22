using AliasPro.API.Database;
using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Utilities;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Models;
using System.Data.Common;

namespace AliasPro.Items.Models
{
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
            Position = new RoomPosition(
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
            Position = new RoomPosition(0, 0, 0.00);
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
            Interaction.Compose(message);
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
        public uint ItemId { get; set; }
        public uint PlayerId { get; set; }
        public string PlayerUsername { get; set; }
        public uint RoomId { get; set; }
        public int Rotation { get; set; }
        public int Mode { get; set; }
        public string ExtraData { get; set; }
        public IRoomPosition Position { get; set; }
        public IItemData ItemData { get; set; }
        public BaseEntity InteractingPlayer { get; set; }

        public IRoom CurrentRoom { get; set; } = null;

        private IItemInteractor _interaction { get; set; }
        private IWiredInteractor _wiredInteraction { get; set; }

        public IItemInteractor Interaction
        {
            get
            {
                if (_interaction == null)
                    _interaction = ItemInteractorUtility.GetItemInteractor(ItemData.InteractionType, this);

                return _interaction;
            }
        }

        public IWiredInteractor WiredInteraction
        {
            get
            {
                if (_wiredInteraction == null)
                    _wiredInteraction = WiredInteractorUtility.GetWiredInteractor(ItemData.WiredInteractionType, this);

                return _wiredInteraction;
            }
        }
    }
}
