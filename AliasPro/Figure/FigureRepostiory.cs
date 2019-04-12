using AliasPro.Figure.Components;
using AliasPro.Players.Models;

namespace AliasPro.Figure
{
    internal class FigureRepostiory
    {
        private readonly FigureDao _figureDao;
        private readonly FigureComponent _figureComponent;

        public FigureRepostiory(FigureDao figureDao)
        {
            _figureDao = figureDao;
            _figureComponent = new FigureComponent();
        }

        internal bool ValidateFigure(string figure, PlayerGender gender) =>
            _figureComponent.Validate(figure, gender);
    }
}
