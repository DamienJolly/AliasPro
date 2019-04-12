namespace AliasPro.Figure
{
    using AliasPro.Players.Models;
    using Models;

    internal class FigureRepostiory
    {
        private readonly FigureDao _figureDao;
        private readonly FigureValidation _antiMutant;

        public FigureRepostiory(FigureDao figureDao)
        {
            _figureDao = figureDao;
            _antiMutant = new FigureValidation();
        }

        internal bool ValidateFigure(string figure, PlayerGender gender) =>
            _antiMutant.Validate(figure, gender);
    }
}
