using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using System.Data.Common;

namespace AliasPro.Players.Models
{
    internal class PlayerPet : IPlayerPet
	{
        public PlayerPet(DbDataReader reader)
        {
            Id = reader.ReadData<int>("id");
			Name = reader.ReadData<string>("name");
			Motto = reader.ReadData<string>("motto");

			switch (reader.ReadData<string>("gender").ToLower())
			{
				case "m": default: Gender = PlayerGender.MALE; break;
				case "f": Gender = PlayerGender.FEMALE; break;
			}

			Type = reader.ReadData<int>("type");
			Race = reader.ReadData<int>("race");
			Colour = reader.ReadData<string>("colour");
		}

		public PlayerPet(int id, string name, string motto, PlayerGender gender, int type, int race, string colour)
		{
			Id = id;
			Name = name;
			Motto = motto;
			Gender = gender;
			Type = type;
			Race = race;
			Colour = colour;
		}

		public void Serialize(ServerPacket message)
		{
			message.WriteInt(Id);
			message.WriteString(Name);
			message.WriteInt(Type);
			message.WriteInt(Race);
			message.WriteString(Colour);
			message.WriteInt(0);
			message.WriteInt(0);
			message.WriteInt(0);
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Motto { get; set; }
		public PlayerGender Gender { get; set; }
		public int Type { get; set; }
		public int Race { get; set; }
		public string Colour { get; set; }
	}
}
