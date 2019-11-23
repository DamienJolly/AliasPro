using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using AliasPro.Players.Types;
using System.Data.Common;

namespace AliasPro.Players.Models
{
    internal class PlayerBot : IPlayerBot
	{
        public PlayerBot(DbDataReader reader)
        {
            Id = reader.ReadData<int>("id");
			Name = reader.ReadData<string>("name");
			Motto = reader.ReadData<string>("motto");

			switch (reader.ReadData<string>("gender").ToLower())
			{
				case "m": default: Gender = PlayerGender.MALE; break;
				case "f": Gender = PlayerGender.FEMALE; break;
			}

			Figure = reader.ReadData<string>("figure");
		}

		/*public PlayerBot(string code, int slot = 0)
		{
			Code = code;
			Slot = slot;
		}*/

		public int Id { get; set; }
		public string Name { get; set; }
		public string Motto { get; set; }
		public PlayerGender Gender { get; set; }
		public string Figure { get; set; }
	}
}
