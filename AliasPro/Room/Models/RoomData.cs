using System.Data.Common;

namespace AliasPro.Room.Models
{
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
    }
}
