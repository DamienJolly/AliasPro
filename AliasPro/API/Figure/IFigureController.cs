using AliasPro.Players.Types;

namespace AliasPro.API.Figure
{
    public interface IFigureController
    {
        bool ValidateFigure(string figure, PlayerGender gender);
    }
}
