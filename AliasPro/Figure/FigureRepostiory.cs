namespace AliasPro.Figure
{
    internal class FigureRepostiory
    {
        private readonly FigureDao _figureDao;

        public FigureRepostiory(FigureDao figureDao)
        {
            _figureDao = figureDao;
        }
    }
}
