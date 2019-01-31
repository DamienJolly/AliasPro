using System.Data.Common;

namespace AliasPro.Landing.Models
{
    using Database;

    internal class HallOfFamer : IHallOfFamer
    {
        internal HallOfFamer(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            Amount = reader.ReadData<int>("amount");
            Username = reader.ReadData<string>("username");
            Figure = reader.ReadData<string>("figure");
        }

        public uint Id { get; set; }
        public int Amount { get; set; }
        public string Username { get; set; }
        public string Figure { get; set; }
    }
}
