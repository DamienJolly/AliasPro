using AliasPro.API.Figure.Models;
using AliasPro.Players.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Figure
{
    public interface IFigureController
    {
        bool ValidateFigure(string figure, PlayerGender gender);
		Task<IDictionary<int, IWardrobeItem>> GetPlayerWardrobeAsync(uint id);
	}
}
