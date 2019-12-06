using AliasPro.API.Database;
using AliasPro.API.Pets.Models;
using System.Collections.Generic;
using System.Data.Common;

namespace AliasPro.Pets.Models
{
	internal class PetData : IPetData
	{
		internal PetData(DbDataReader reader)
		{
			Type = reader.ReadData<int>("pet_type");
			Name = reader.ReadData<string>("pet_name");
			Breeds = new List<IPetBreed>();
		}

		public int Type { get; set; }
		public string Name { get; set; }
		public IList<IPetBreed> Breeds { get; set; }
	}
}
