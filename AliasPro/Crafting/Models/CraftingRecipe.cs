using AliasPro.API.Crafting.Models;
using AliasPro.API.Database;
using System.Collections.Generic;
using System.Data.Common;

namespace AliasPro.Crafting.Models
{
	internal class CraftingRecipe : ICraftingRecipe
	{
		internal CraftingRecipe(DbDataReader reader)
		{
			Id = reader.ReadData<int>("id");
			RewardId = reader.ReadData<int>("reward_id");
			Name = reader.ReadData<string>("reward_name");
			Secret = reader.ReadData<string>("secret") == "1";
			Achievement = reader.ReadData<string>("achievement");
			Limited = reader.ReadData<string>("limited") == "1";
			Remaining = reader.ReadData<int>("remaining");
			Ingredients = new Dictionary<int, ICraftingIngredient>();
		}

		public int Id { get; set; }
		public int RewardId { get; set; }
		public string Name { get; set; }
		public bool Secret { get; set; }
		public string Achievement { get; set; }
		public bool Limited { get; set; }
		public int Remaining { get; set; }
		public IDictionary<int, ICraftingIngredient> Ingredients { get; set; }

		public bool CanBeCrafted =>
			!Limited || Remaining > 0;

		public bool TryGetIngredient(int ingredientId, out ICraftingIngredient ingredient) =>
			Ingredients.TryGetValue(ingredientId, out ingredient);

		public bool TryAddIngredient(ICraftingIngredient ingredient) =>
			Ingredients.TryAdd(ingredient.Id, ingredient);
	}
}
