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

		public FigureController(FigureDao figureDao)
        {
			_figureDao = figureDao;
			_figureComponent = new FigureComponent();
		}
		public bool ValidateFigure(string figure, PlayerGender gender) =>
			_figureComponent.Validate(figure, gender);
			
		public async Task<IDictionary<int, IWardrobeItem>> GetPlayerWardrobeAsync(uint id) =>
			await _figureDao.GetPlayerWardrobeAsync(id);
	}
}