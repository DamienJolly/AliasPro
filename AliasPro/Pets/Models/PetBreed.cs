using AliasPro.API.Database;
using AliasPro.API.Pets.Models;
using System.Data.Common;

namespace AliasPro.Pets.Models
{
	internal class PetBreed : IPetBreed
	{
		internal PetBreed(DbDataReader reader)
		{
			ColourOne = reader.ReadData<int>("colour_one");
			ColourTwo = reader.ReadData<int>("colour_two");
			HasColourOne = reader.ReadData<bool>("has_colour_one");
			HasColourTwo = reader.ReadData<bool>("has_colour_two");
		}

		public int ColourOne { get; set; }
		public int ColourTwo { get; set; }
		public bool HasColourOne { get; set; }
		public bool HasColourTwo { get; set; }
	}
}
