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
            Score = reader.ReadData<int>("score");
            OwnerId = reader.ReadData<int>("owner");
            Name = reader.ReadData<string>("name");
            Password = reader.ReadData<string>("password");
            ModelName = reader.ReadData<string>("model_name");
        }

        public uint Id { get; set; }
        public int Score { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ModelName { get; set; }
        public int UsersNow { get; set; } = 0;
        public int CategoryId { get; set; } = 5; //Chat and discussion

        public int EnumType
        {
            get
            {
                int type = 0;
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

        public void Compose(ServerPacket serverPacket)
        {
            serverPacket.WriteInt(Id);
            serverPacket.WriteString(Name);
            serverPacket.WriteInt(OwnerId);
            serverPacket.WriteString("Damien");
            serverPacket.WriteInt(1);
            serverPacket.WriteInt(UsersNow);
            serverPacket.WriteInt(25);
            serverPacket.WriteString("");
            serverPacket.WriteInt(0);
            serverPacket.WriteInt(Score);
            serverPacket.WriteInt(0);
            serverPacket.WriteInt(1);
            serverPacket.WriteInt(0);
            serverPacket.WriteInt(EnumType);
        }
    }

    public interface IRoomData
    {
        uint Id { get; set; }
        int Score { get; set; }
        int OwnerId { get; set; }
        string Name { get; set; }
        string Password { get; set; }
        string ModelName { get; set; }
        int UsersNow { get; set; }
        int CategoryId { get; set; }
        int EnumType { get; }

        void Compose(ServerPacket serverPacket);
    }
}
