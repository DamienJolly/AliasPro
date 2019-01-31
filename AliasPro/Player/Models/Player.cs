using System.Data.Common;

namespace AliasPro.Player.Models
{
    using Database;

    internal class Player : IPlayer
    {
        internal Player(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            Credits = reader.ReadData<int>("credits");
            Rank = reader.ReadData<int>("rank");
            Username = reader.ReadData<string>("username");
            SsoTicket = reader.ReadData<string>("auth_ticket");
            Figure = reader.ReadData<string>("figure");
            Gender = reader.ReadData<string>("gender");
            Motto = reader.ReadData<string>("motto");
        }

        public uint Id { get; set; }
        public int Credits { get; set; }
        public int Rank { get; set; }
        public string Username { get; set; }
        public string SsoTicket { get; set; }
        public string Figure { get; set; }
        public string Gender { get; set; }
        public string Motto { get; set; }
    }
}
