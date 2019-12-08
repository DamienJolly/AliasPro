using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using AliasPro.Utilities;
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
			Experience = reader.ReadData<int>("experience");
			Happyness = reader.ReadData<int>("happyness");
			Energy = reader.ReadData<int>("energy");
			Hunger = reader.ReadData<int>("hunger");
			Thirst = reader.ReadData<int>("thirst");
			Respect = reader.ReadData<int>("respect");
			Created = reader.ReadData<int>("created");
		}

		public PlayerPet(int id, string name, int type, int race, string colour)
		{
			Id = id;
			Name = name;
			Type = type;
			Race = race;
			Colour = colour;
			Experience = 0;
			Happyness = 100;
			Energy = 100;
			Hunger = 0;
			Thirst = 0;
			Respect = 0;
			Created = (int)UnixTimestamp.Now;
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
		public int Experience { get; set; }
		public int Happyness { get; set; }
		public int Energy { get; set; }
		public int Hunger { get; set; }
		public int Thirst { get; set; }
		public int Respect { get; set; }
		public int Created { get; set; }
	}
}
