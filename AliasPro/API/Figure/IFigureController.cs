using AliasPro.API.Figure.Models;
using AliasPro.Players.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Figure
{
    public interface IFigureController
    {
        bool ValidateFigure(string figure, PlayerGender gender);
		Task AddWardrobeItemAsync(uint playerId, IWardrobeItem item);
		Task UpdateWardrobeItemAsync(uint playerId, IWardrobeItem item);
		Task<IDictionary<int, IWardrobeItem>> GetPlayerWardrobeAsync(uint id);
		Task<IDictionary<int, IClothingItem>> GetPlayerClothingAsync(uint id);
	}
}
