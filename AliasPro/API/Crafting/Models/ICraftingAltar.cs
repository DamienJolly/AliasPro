using AliasPro.API.Players.Models;
using System.Collections.Generic;

namespace AliasPro.API.Crafting.Models
{
	public interface ICraftingAltar
	{
		IDictionary<int, ICraftingIngredient> Ingredients { get; }

		bool TryGetRecipe(int recipeId, out ICraftingRecipe recipe);
		bool TryGetRecipe(string recipeName, out ICraftingRecipe recipe);
		bool TryGetRecipe(IDictionary<int, int> playerItems, out ICraftingRecipe recipe);
		bool TryAddRecipe(ICraftingRecipe recipe);
		ICollection<ICraftingRecipe> GetRecipesForPlayer(IPlayer player);
		IDictionary<ICraftingRecipe, bool> MatchRecipes(IDictionary<int, int> playerItems);
	}
}
