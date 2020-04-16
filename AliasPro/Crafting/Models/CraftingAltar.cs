using AliasPro.API.Crafting.Models;
using AliasPro.API.Players.Models;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Crafting.Models
{
	internal class CraftingAltar : ICraftingAltar
	{
		public CraftingAltar(int itemId)
		{
			BaseItemId = itemId;
			Recipes = new Dictionary<int, ICraftingRecipe>();
		}

		public int BaseItemId { get; set; }
		public IDictionary<int, ICraftingRecipe> Recipes { get; set; }

		public bool TryGetRecipe(int recipeId, out ICraftingRecipe recipe) => 
			Recipes.TryGetValue(recipeId, out recipe);

		public bool TryGetRecipe(string recipeName, out ICraftingRecipe recipe)
		{
			recipe = null;
			foreach (ICraftingRecipe r in Recipes.Values)
			{
				if (r.Name == recipeName)
				{
					recipe = r;
					return true;
				}
			}
			return false;
		}

		public bool TryGetRecipe(IDictionary<int, int> playerItems, out ICraftingRecipe recipe)
		{
			recipe = null;
			foreach (ICraftingRecipe r in Recipes.Values)
			{
				recipe = r;
				foreach (ICraftingIngredient i in r.Ingredients.Values)
				{
					if (!(playerItems.ContainsKey(i.Id) && playerItems[i.Id] == i.Amount))
					{
						recipe = null;
						break;
					}
				}

				if (recipe != null)
					return true;
			}
			return false;
		}

		public bool TryAddRecipe(ICraftingRecipe recipe) =>
			Recipes.TryAdd(recipe.Id, recipe);

		public ICollection<ICraftingRecipe> GetRecipesForPlayer(IPlayer player)
		{
			IList<ICraftingRecipe> recipeList = new List<ICraftingRecipe>();
			foreach (ICraftingRecipe recipe in Recipes.Values)
			{
				if (!recipe.Secret || player.Recipe.TryGetRecipe(recipe.Id))
					recipeList.Add(recipe);
			}

			return recipeList;
		}

		public IDictionary<ICraftingRecipe, bool> MatchRecipes(IDictionary<int, int> playerItems)
		{
			IDictionary<ICraftingRecipe, bool> foundRecepies = new Dictionary<ICraftingRecipe, bool>();

			foreach (ICraftingRecipe recipe in Recipes.Values)
			{
				if (!recipe.Secret || !recipe.CanBeCrafted)
					continue;

				bool contains = true;
				bool equals = playerItems.Count == recipe.Ingredients.Count;

				foreach (var playerItem in playerItems)
				{
					if (!contains)
						break;

					if (recipe.TryGetIngredient(playerItem.Key, out ICraftingIngredient ingredient))
					{
						if (ingredient.Amount == playerItem.Value)
							continue;

						equals = false;

						if (ingredient.Amount >= playerItem.Value)
							continue;
					}

					contains = false;
				}

				if (contains)
					foundRecepies.Add(recipe, equals);
			}

			return foundRecepies;
		}


		public ICollection<ICraftingIngredient> Ingredients
		{
			get
			{
				IList<ICraftingIngredient> ingrdientList = new List<ICraftingIngredient>();
				foreach (CraftingRecipe recipe in Recipes.Values)
				{
					foreach (ICraftingIngredient ingrdient in recipe.Ingredients.Values)
					{
						ingrdientList.Add(ingrdient);
					}
				}

				return ingrdientList;
			}
		}
	}
}
