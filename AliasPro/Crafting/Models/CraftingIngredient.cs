using AliasPro.API.Crafting.Models;
using AliasPro.API.Database;
using System.Data.Common;

namespace AliasPro.Crafting.Models
{
	internal class CraftingIngredient : ICraftingIngredient
	{
		internal CraftingIngredient(DbDataReader reader)
		{
			Id = reader.ReadData<int>("ingredient_id");
			Name = reader.ReadData<string>("ingredient_name");
			Amount = reader.ReadData<int>("amount");
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public int Amount { get; set; }
	}
}
