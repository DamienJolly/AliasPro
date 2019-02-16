using System.Data.Common;

namespace AliasPro.Room.Models
{
    using Network.Protocol;
    using Database;

    internal class RoomData : IRoomData
    {
        internal RoomData(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            OwnerId = reader.ReadData<int>("owner");
            Name = reader.ReadData<string>("name");
            Description = reader.ReadData<string>("caption");
            Password = reader.ReadData<string>("password");
            ModelName = reader.ReadData<string>("model_name");
            MaxUsers = reader.ReadData<int>("max_users");
            TradeType = reader.ReadData<int>("trade_type");
            CategoryId = reader.ReadData<int>("category_id");
            Score = reader.ReadData<int>("score");
        }

        internal RoomData(uint ownerId, string name, string description, string modelName, int maxUsers, int tradeType, int categoryId)
        {
            Id = 0;
            OwnerId = (int)ownerId;
            Name = name;
            Description = description;
            Password = "";
            ModelName = modelName;
            MaxUsers = maxUsers;
            TradeType = tradeType;
            CategoryId = categoryId;
            Score = 0;
        }

        public void Compose(ServerPacket serverPacket)
        {
            serverPacket.WriteInt(Id);
            serverPacket.WriteString(Name);
            serverPacket.WriteInt(OwnerId);
            serverPacket.WriteString("Damien");
            serverPacket.WriteInt(DoorState);
            serverPacket.WriteInt(UsersNow);
            serverPacket.WriteInt(MaxUsers);
            serverPacket.WriteString(Description);
            serverPacket.WriteInt(TradeType);
            serverPacket.WriteInt(Score);
            serverPacket.WriteInt(0); // dunno?
            serverPacket.WriteInt(CategoryId);
            serverPacket.WriteInt(0); // tags
            serverPacket.WriteInt(EnumType);
        }

        public uint Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string ModelName { get; set; }
        public int UsersNow { get; set; } = 0;
        public int MaxUsers { get; set; }
        public int CategoryId { get; set; }
        public int TradeType { get; set; }
        public int DoorState { get; set; } = 0;
        public int Score { get; set; }

        public int EnumType
        {
            get
            {
                int type = 8;
                return type;
                /*  int RoomType = 0;
               if (Data.Image != null)
                   RoomType += 1;
			   if (Data.Group != null)
				   RoomType += 2;
			   if (Data.Promotion != null)
				   RoomType += 4;
			   if (Data.Type == "private")
				   RoomType += 8;
               if (Data.Allowpets)
				   RoomType += 16;
				   */
            }
        }

        public IRoomSettings Settings { get; set; }
    }

    public interface IRoomData
    {
        void Compose(ServerPacket serverPacket);

        uint Id { get; set; }
        int OwnerId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Password { get; set; }
        string ModelName { get; set; }
        int UsersNow { get; set; }
        int MaxUsers { get; set; }
        int CategoryId { get; set; }
        int TradeType { get; set; }
        int DoorState { get; set; }
        int Score { get; set; }

        IRoomSettings Settings { get; set; }
    }
}
