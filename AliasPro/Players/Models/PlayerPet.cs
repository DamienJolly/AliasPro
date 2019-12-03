using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using System.Data.Common;

namespace AliasPro.Players.Models
{
    internal class PlayerPet : IPlayerPet
	{
        public PlayerPet(DbDataReader reader)
        {
            Id = reader.ReadData<int>("id");
			Name = reader.ReadData<string>("name");
			Type = reader.ReadData<int>("type");
			Race = reader.ReadData<int>("race");
			Colour = reader.ReadData<string>("colour");
		}

		public PlayerPet(int id, string name, int type, int race, string colour)
		{
			Id = id;
			Name = name;
			Type = type;
			Race = race;
			Colour = colour;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public int Type { get; set; }
		public int Race { get; set; }
		public string Colour { get; set; }
	}
}
