using System.Collections.Generic;

namespace AliasPro.API.Crafting.Models
{
	public interface ICraftingRecipe
	{
		int Id { get; set; }
		int RewardId { get; set; }
		string Name { get; set; }
		bool Secret { get; set; }
		string Achievement { get; set; }
		bool Limited { get; set; }
		int Remaining { get; set; }
		IDictionary<int, ICraftingIngredient> Ingredients { get; set; }
		bool CanBeCrafted { get; }

		bool TryGetIngredient(int ingredientId, out ICraftingIngredient ingredient);
		bool TryAddIngredient(ICraftingIngredient ingredient);
	}
}
