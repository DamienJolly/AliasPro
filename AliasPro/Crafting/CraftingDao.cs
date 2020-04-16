using AliasPro.API.Configuration;
using AliasPro.API.Crafting.Models;
using AliasPro.API.Database;
using AliasPro.Crafting.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Crafting
{
	internal class CraftingDao : BaseDao
	{
		public CraftingDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
			: base(logger, configurationController)
		{

		}

		public async Task<IDictionary<int, ICraftingAltar>> ReadCraftingAltars()
		{
			IDictionary<int, ICraftingAltar> altars = new Dictionary<int, ICraftingAltar>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						int altarId = reader.ReadData<int>("altar_id");
						if (!altars.ContainsKey(altarId))
							altars.Add(altarId, new CraftingAltar(altarId));

						if (!altars.TryGetValue(altarId, out ICraftingAltar altar))
							continue;

						int recipeId = reader.ReadData<int>("recipe_id");
						if (!altar.TryGetRecipe(recipeId, out ICraftingRecipe recipe))
						{
							recipe = new CraftingRecipe(reader);
							altar.TryAddRecipe(recipe);
						}

						int ingredientId = reader.ReadData<int>("ingredient_id");
						int amount = reader.ReadData<int>("amount");
						if (!recipe.TryGetIngredient(ingredientId, out ICraftingIngredient ingredient))
						{
							ingredient = new CraftingIngredient(reader);
							recipe.TryAddIngredient(ingredient);
						}
					}
				}, "SELECT * FROM crafting_altars " +
					"INNER JOIN crafting_recipes ON crafting_altars.recipe_id = crafting_recipes.id " +
					"INNER JOIN crafting_recipes_ingredients ON crafting_recipes.id = crafting_recipes_ingredients.recipe_id " +
					"WHERE crafting_recipes.enabled = '1' ORDER BY altar_id ASC;");
			});
			return altars;
		}

		public async Task UpdateRecipeAsync(ICraftingRecipe recipe)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "UPDATE `crafting_recipes` SET `remaining` = @1 WHERE `id` = @0;",
					recipe.Id, recipe.Remaining);
			});
		}
	}
}
