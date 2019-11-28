using AliasPro.API.Figure;
using AliasPro.API.Figure.Models;
using AliasPro.Figure.Components;
using AliasPro.Players.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Figure
{
    internal class FigureController : IFigureController
    {
        private readonly FigureDao _figureDao;
		private readonly FigureComponent _figureComponent;
		private IDictionary<int, IClothingItem> _clothingItems;

		public FigureController(FigureDao figureDao)
        {
			_figureDao = figureDao;
			_figureComponent = new FigureComponent();
			_clothingItems = new Dictionary<int, IClothingItem>();

			InitializeClothing();
		}

		public async void InitializeClothing()
		{
			_clothingItems = await _figureDao.GetClothingItemsAsync();
		}

		public bool TryGetClothingItem(int clothingId, out IClothingItem item) =>
			_clothingItems.TryGetValue(clothingId, out item);

		public bool ValidateFigure(string figure, PlayerGender gender) =>
			_figureComponent.Validate(figure, gender);

		public async Task AddWardrobeItemAsync(uint playerId, IWardrobeItem item) =>
			await _figureDao.AddWardrobeItemAsync(playerId, item);

		public async Task UpdateWardrobeItemAsync(uint playerId, IWardrobeItem item) =>
			await _figureDao.UpdateWardrobeItemAsync(playerId, item);

		public async Task<IDictionary<int, IWardrobeItem>> GetPlayerWardrobeAsync(uint id) =>
			await _figureDao.GetPlayerWardrobeAsync(id);


		public async Task<IDictionary<int, IClothingItem>> GetPlayerClothingAsync(uint id) =>
			await _figureDao.GetPlayerClothingAsync(id);

		public async Task AddClothingItemAsync(uint playerId, IClothingItem item) =>
			await _figureDao.AddClothingItemAsync(playerId, item);
	}
}