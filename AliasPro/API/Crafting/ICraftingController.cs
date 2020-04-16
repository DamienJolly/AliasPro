using AliasPro.API.Crafting.Models;
using System.Threading.Tasks;

namespace AliasPro.API.Crafting
{
    public interface ICraftingController
	{
		void InitializeCrafting();
		Task UpdateRecipeAsync(ICraftingRecipe recipe);
		bool TryGetAltar(int itemId, out ICraftingAltar altar);
		bool TryGetRecipe(string recipeName, out ICraftingRecipe recipe);
	}
}
