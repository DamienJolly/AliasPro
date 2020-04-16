using System.Collections.Generic;

namespace AliasPro.Players.Components
{
    public class RecipeComponent
	{
		private readonly IList<int> _recipes;

		public RecipeComponent(
			IList<int> recipes)
        {
			_recipes = recipes;
		}

		public bool TryGetRecipe(int recipeId) =>
			_recipes.Contains(recipeId);

		public bool TryAdd(int recipeId)
		{
			if (_recipes.Contains(recipeId))
				return false;

			_recipes.Add(recipeId);
			return true;
		}
	}
}
