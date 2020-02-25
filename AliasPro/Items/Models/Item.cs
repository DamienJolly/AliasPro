using AliasPro.API.Database;
using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Utilities;
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
            PlayerUsername = reader.ReadData<string>("username");
            RoomId = reader.ReadData<uint>("room_id");
            Rotation = reader.ReadData<int>("rot");
            ExtraData = reader.ReadData<string>("extra_data");
            Position = new RoomPosition(
                reader.ReadData<int>("x"),
                reader.ReadData<int>("y"),
                reader.ReadData<double>("z"));
            WallCord = reader.ReadData<string>("wall_cord");
        }

        internal Item(uint itemId, uint playerId, string playerName, string extraData, IItemData itemData)
        {
            Id = 0;
            ItemId = itemId;
            PlayerId = playerId;
            PlayerUsername = playerName;
            RoomId = 0;
            Rotation = 0;
            ExtraData = extraData;
            Position = new RoomPosition(0, 0, 0.00);
            WallCord = string.Empty;
            ItemData = itemData;
        }

        public void ComposeFloorItem(ServerMessage message)
        {
            message.WriteInt((int)Id);
            message.WriteInt((int)ItemData.SpriteId);
            message.WriteInt(Position.X);
            message.WriteInt(Position.Y);
            message.WriteInt(Rotation);
            message.WriteString(string.Format("{0:0.00}", Position.Z.ToString()));
            message.WriteString("");
            Interaction.Compose(message);
            message.WriteInt(-1);
            message.WriteInt(ItemData.Modes > 1 ? 1 : 0);
            message.WriteInt((int)PlayerId); // -12345678 = builders club
        }

        public void ComposeWallItem(ServerMessage message)
        {
            message.WriteString(Id + "");
            message.WriteInt((int)ItemData.SpriteId);
            message.WriteString(WallCord);
            message.WriteString(ExtraData);
            message.WriteInt(-1);
            message.WriteInt(ItemData.Modes > 1 ? 1 : 0);
            message.WriteInt((int)PlayerId); // -12345678 = builders club
        }

        public uint Id { get; set; }
        public uint ItemId { get; set; }
        public uint PlayerId { get; set; }
        public string PlayerUsername { get; set; }
        public uint RoomId { get; set; }
        public int Rotation { get; set; }
        public string ExtraData { get; set; }
        public IRoomPosition Position { get; set; }
        public string WallCord { get; set; }
        public IItemData ItemData { get; set; }
        public BaseEntity InteractingPlayer { get; set; }
		public BaseEntity InteractingPlayerTwo { get; set; }

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
			set { }
		}

        public IWiredInteractor WiredInteraction
        {
            get
            {
                if (_wiredInteraction == null)
                    _wiredInteraction = WiredInteractorUtility.GetWiredInteractor(ItemData.WiredInteractionType, this);

                return _wiredInteraction;
            }
			set { }
		}
    }
}
