using AliasPro.API.Player.Models;
using AliasPro.Database;
using System.Data.Common;

namespace AliasPro.Player.Models
{
    internal class PlayerData : IPlayerData
    {
        internal PlayerData(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            Credits = reader.ReadData<int>("credits");
            Rank = reader.ReadData<int>("rank");
            Username = reader.ReadData<string>("username");
            Figure = reader.ReadData<string>("figure");
            
            switch (reader.ReadData<string>("gender").ToLower())
            {
                case "m": default: Gender = PlayerGender.MALE; break;
                case "f": Gender = PlayerGender.FEMALE; break;
            }
            
            Motto = reader.ReadData<string>("motto");
            Online = reader.ReadData<bool>("is_online");
        }

        internal PlayerData(IPlayerData data)
        {
            Id = data.Id;
            Credits = data.Credits;
            Rank = data.Rank;
            Username = data.Username;
            Figure = data.Figure;
            Gender = data.Gender;
            Motto = data.Motto;
            Online = true;
        }

        public uint Id { get; set; }
        public int Credits { get; set; }
        public int Rank { get; set; }
        public string Username { get; set; }
        public string Figure { get; set; }
        public PlayerGender Gender { get; set; }
        public string Motto { get; set; }

        public bool Online { get; set; }
    }
}
