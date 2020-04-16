using AliasPro.API.Crafting;
using AliasPro.API.Crafting.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Crafting
{
	internal class CraftingController : ICraftingController
	{
		private readonly CraftingDao _craftingDao;

		private IDictionary<int, ICraftingAltar> _altars;

		public CraftingController(CraftingDao craftingDao)
		{
			_craftingDao = craftingDao;

			_altars = new Dictionary<int, ICraftingAltar>();

			InitializeCrafting();
		}

		public async void InitializeCrafting()
		{
			_altars = await _craftingDao.ReadCraftingAltars();
		}

		public async Task UpdateRecipeAsync(ICraftingRecipe recipe) =>
			await _craftingDao.UpdateRecipeAsync(recipe);

		public bool TryGetAltar(int itemId, out ICraftingAltar altar) =>
			_altars.TryGetValue(itemId, out altar);

		public bool TryGetRecipe(string recipeName, out ICraftingRecipe recipe)
		{
			recipe = null;
			foreach (ICraftingAltar altar in _altars.Values)
			{
				if (altar.TryGetRecipe(recipeName, out recipe))
					return true;
			}
			return false;
		}
	}
}
