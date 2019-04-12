using AliasPro.Players.Models;

namespace AliasPro.Figure
{
    internal class FigureController : IFigureController
    {
        private readonly FigureRepostiory _figureRepostiory;

        public FigureController(FigureRepostiory figureRepostiory)
        {
            _figureRepostiory = figureRepostiory;
        }

        public bool ValidateFigure(string figure, PlayerGender gender) =>
            _figureRepostiory.ValidateFigure(figure, gender);
    }

    public interface IFigureController
    {
        bool ValidateFigure(string figure, PlayerGender gender);
    }
}