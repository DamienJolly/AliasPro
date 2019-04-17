using AliasPro.API.Database;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Protocol;
using System.Collections.Generic;
using System.Data.Common;

namespace AliasPro.Rooms.Models
{
    internal class RoomData : IRoomData
    {
        internal RoomData(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            OwnerId = reader.ReadData<int>("owner");
            OwnerName = reader.ReadData<string>("username");
            Name = reader.ReadData<string>("name");
            Description = reader.ReadData<string>("caption");
            Password = reader.ReadData<string>("password");
            ModelName = reader.ReadData<string>("model_name");
            MaxUsers = reader.ReadData<int>("max_users");
            TradeType = reader.ReadData<int>("trade_type");
            CategoryId = reader.ReadData<int>("category_id");
            Score = reader.ReadData<int>("score");
            Tags = new List<string>();

            string tags = reader.ReadData<string>("tags");
            foreach (string tag in tags.Split(";"))
            {
                if (string.IsNullOrWhiteSpace(tag))
                    continue;

                Tags.Add(tag);
            }
        }

        internal RoomData(IRoomData data)
        {
            Id = data.Id;
            OwnerId = data.OwnerId;
            OwnerName = data.OwnerName;
            Name = data.Name;
            Description = data.Description;
            Password = data.Password;
            ModelName = data.ModelName;
            MaxUsers = data.MaxUsers;
            TradeType = data.TradeType;
            CategoryId = data.CategoryId;
            Score = data.Score;
            Tags = data.Tags;
        }

        //todo: remove
        internal RoomData(uint ownerId, string ownerName, string name, string description, string modelName, int maxUsers, int tradeType, int categoryId)
        {
            Id = 0;
            OwnerId = (int)ownerId;
            OwnerName = ownerName;
            Name = name;
            Description = description;
            Password = "";
            ModelName = modelName;
            MaxUsers = maxUsers;
            TradeType = tradeType;
            CategoryId = categoryId;
            Score = 0;
            Tags = new List<string>();
        }

        public void Compose(ServerPacket serverPacket)
        {
            serverPacket.WriteInt(Id);
            serverPacket.WriteString(Name);
            serverPacket.WriteInt(OwnerId);
            serverPacket.WriteString(OwnerName);
            serverPacket.WriteInt(DoorState);
            serverPacket.WriteInt(UsersNow);
            serverPacket.WriteInt(MaxUsers);
            serverPacket.WriteString(Description);
            serverPacket.WriteInt(TradeType);
            serverPacket.WriteInt(Score);
            serverPacket.WriteInt(0); // dunno?
            serverPacket.WriteInt(CategoryId);
            serverPacket.WriteInt(Tags.Count);
            foreach (string tag in Tags)
                serverPacket.WriteString(tag);
            serverPacket.WriteInt(EnumType);
        }

        public uint Id { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
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
        public IList<string> Tags { get; set; }

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

        public IRoomModel RoomModel { get; set; }
        public IRoomSettings Settings { get; set; }
    }
}
