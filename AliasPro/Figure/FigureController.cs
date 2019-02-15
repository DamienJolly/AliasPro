namespace AliasPro.Figure
{
    internal class FigureController : IFigureController
    {
        private readonly FigureRepostiory _figureRepostiory;

        public FigureController(FigureRepostiory figureRepostiory)
        {
            _figureRepostiory = figureRepostiory;
        }
    }

    public interface IFigureController
    {
    }
}